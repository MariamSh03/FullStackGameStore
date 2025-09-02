using System.Net.Http.Json;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Exceptions;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;
using QuestPDF.Fluent;

namespace AdminPanel.Bll.Services;
public class OrderService : IOrderService
{
    private readonly IGameRepository _gameRepository;
    private readonly IGenericRepository<OrderEntity> _orderRepository;
    private readonly IGenericRepository<OrderGameEntity> _orderGameRepository;
    private readonly HttpClient _httpClient;

    public OrderService(IGameRepository gameRepository, IGenericRepository<OrderEntity> orderRepository, IGenericRepository<OrderGameEntity> orderGameRepository, HttpClient httpClient)
    {
        _gameRepository = gameRepository;
        _orderRepository = orderRepository;
        _orderGameRepository = orderGameRepository;
        _httpClient = httpClient;
    }

    public async Task AddGameToCartAsync(Guid gameId)
    {
        // Get the game from repository
        var game = await _gameRepository.GetByIdAsync(gameId) ?? throw new GameNotFoundException("Game not found");

        // Prevent buying deleted games
        if (game.IsDeleted)
        {
            throw new GameServiceException("Cannot purchase deleted games");
        }

        if (game.UnitInStock <= 0)
        {
            throw new GameServiceException("Game is out of stock");
        }

        // Get or create open cart
        var cart = await GetOrCreateCartAsync();

        // Check if game already in cart
        var cartItems = await _orderGameRepository.FindAsync(og =>
            og.OrderId == cart.Id && og.ProductId == gameId);
        var existingOrderGame = cartItems.FirstOrDefault();

        if (existingOrderGame != null)
        {
            // Game already in cart, increment quantity
            if (existingOrderGame.Quantity > game.UnitInStock)
            {
                throw new GameServiceException("Cannot order more games than available in stock");
            }

            existingOrderGame.Quantity++;
            game.UnitInStock--;
            await _orderGameRepository.UpdateAsync(existingOrderGame);
        }
        else
        {
            // Add new game to cart
            var orderGame = new OrderGameEntity
            {
                OrderId = cart.Id,
                ProductId = gameId,
                Price = game.Price,
                Quantity = 1,
                Discount = 0,
            };

            await _orderGameRepository.AddAsync(orderGame);
        }
    }

    public async Task<OrderDto> GetOrderByIdAsync(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        return order == null
            ? throw new OrderNotFoundException("Order not found")
            : new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Date = order.Date,
            };
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();

        return orders.Select(order => new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Date = order.Date,
        }).ToList();
    }

    public async Task RemoveGameFromCartAsync(string key)
    {
        var cart = await GetOrCreateCartAsync();
        var games = await _gameRepository.FindAsync(g => g.Key == key);

        foreach (var game in games)
        {
            Guid gameId = game.Id;
            var cartItem = await _orderGameRepository.FindAsync(og => og.OrderId == cart.Id && og.ProductId == gameId);
            var gameToRemove = cartItem.FirstOrDefault() ?? throw new GameNotFoundException("Game not found in cart");
            await _orderGameRepository.DeleteAsync(gameToRemove);
        }
    }

    public async Task<IEnumerable<OrderGameDto>> GetCartAsync()
    {
        var cart = await GetOpenCartAsync();
        if (cart == null)
        {
            return Enumerable.Empty<OrderGameDto>();
        }

        var cartItems = await _orderGameRepository.FindAsync(og => og.OrderId == cart.Id);
        var result = new List<OrderGameDto>();

        foreach (var item in cartItems)
        {
            var game = await _gameRepository.GetByIdAsync(item.ProductId);
            if (game == null)
            {
                continue;
            }

            result.Add(new OrderGameDto
            {
                GameId = game.Id,
                GameName = game.Name,
                Price = item.Price,
                Quantity = item.Quantity,
            });
        }

        return result;
    }

    public async Task<IEnumerable<OrderGameDto>> GetOrderDetailsAsync(Guid orderId)
    {
        var orderGames = await _orderGameRepository.FindAsync(og => og.OrderId == orderId);

        if (!orderGames.Any())
        {
            throw new OrderNotFoundException($"Order details for order ID {orderId} not found.");
        }

        var gameDtos = new List<OrderGameDto>();

        foreach (var og in orderGames)
        {
            var game = await _gameRepository.GetByIdAsync(og.ProductId);
            if (game == null)
            {
                continue;
            }

            gameDtos.Add(new OrderGameDto
            {
                GameId = game.Id,
                GameName = game.Name,
                Price = og.Price,
                Quantity = og.Quantity,
            });
        }

        return gameDtos;
    }

    public async Task<PaymentMethodsDto> GetPaymentMethodsAsync()
    {
        // In a real application, these would typically come from a database or configuration
        return await Task.FromResult(new PaymentMethodsDto
        {
            PaymentMethods = new List<PaymentMethodDto>
            {
                new()
                {
                        ImageUrl = "image link1",
                        Title = "Bank",
                        Description = "Some text 1",
                },

                new()
                {
                        ImageUrl = "image link2",
                        Title = "IBox terminal",
                        Description = "Some text 2",
                },

                new()
                {
                    ImageUrl = "image link3",
                    Title = "Visa",
                    Description = "Some text 3",
                },
            },
        });
    }

    // US17 - Update order detail quantity
    public async Task UpdateOrderDetailQuantityAsync(Guid detailId, int quantity)
    {
        if (quantity < 0)
        {
            throw new GameServiceException("Quantity must be greater than 0");
        }

        var orderGames = await _orderGameRepository.FindAsync(og =>
            og.OrderId == detailId || og.ProductId == detailId);
        var orderGame = orderGames.FirstOrDefault()
            ?? throw new OrderNotFoundException("Order detail not found");

        // Check if enough stock is available
        var game = await _gameRepository.GetByIdAsync(orderGame.ProductId)
            ?? throw new GameNotFoundException("Game not found");

        if (quantity > game.UnitInStock + orderGame.Quantity)
        {
            throw new GameServiceException("Not enough stock available");
        }

        // Update stock
        game.UnitInStock += orderGame.Quantity - quantity;
        orderGame.Quantity = quantity;

        await _orderGameRepository.UpdateAsync(orderGame);
        await _gameRepository.UpdateAsync(game);
    }

    // US18 - Delete order detail
    public async Task DeleteOrderDetailAsync(Guid detailId)
    {
        var orderGames = await _orderGameRepository.FindAsync(og =>
            og.OrderId == detailId || og.ProductId == detailId);
        var orderGame = orderGames.FirstOrDefault()
            ?? throw new OrderNotFoundException("Order detail not found");

        // Return stock to game
        var game = await _gameRepository.GetByIdAsync(orderGame.ProductId);
        if (game != null)
        {
            game.UnitInStock += orderGame.Quantity;
            await _gameRepository.UpdateAsync(game);
        }

        await _orderGameRepository.DeleteAsync(orderGame);
    }

    // US19 - Ship order
    public async Task ShipOrderAsync(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId)
            ?? throw new OrderNotFoundException("Order not found");

        if (order.Status != OrderStatus.Paid)
        {
            throw new GameServiceException("Only paid orders can be shipped");
        }

        order.Status = OrderStatus.Shipped;
        await _orderRepository.UpdateAsync(order);
    }

    // US20 - Add game as order detail
    public async Task AddGameToOrderAsync(Guid orderId, string gameKey)
    {
        _ = await _orderRepository.GetByIdAsync(orderId)
            ?? throw new OrderNotFoundException("Order not found");
        var games = await _gameRepository.FindAsync(g => g.Key == gameKey);
        var game = games.FirstOrDefault()
            ?? throw new GameNotFoundException("Game not found");

        // Prevent buying deleted games
        if (game.IsDeleted)
        {
            throw new GameServiceException("Cannot purchase deleted games");
        }

        if (game.UnitInStock <= 0)
        {
            throw new GameServiceException("Game is out of stock");
        }

        // Check if game already exists in this order
        var existingOrderGames = await _orderGameRepository.FindAsync(og =>
            og.OrderId == orderId && og.ProductId == game.Id);
        var existingOrderGame = existingOrderGames.FirstOrDefault();

        if (existingOrderGame != null)
        {
            // Game already in order, increment quantity
            existingOrderGame.Quantity++;
            await _orderGameRepository.UpdateAsync(existingOrderGame);
        }
        else
        {
            // Add new game to order
            var orderGame = new OrderGameEntity
            {
                OrderId = orderId,
                ProductId = game.Id,
                Price = game.Price,
                Quantity = 1,
                Discount = game.Discount,
            };
            await _orderGameRepository.AddAsync(orderGame);
        }

        // Update game stock
        game.UnitInStock--;
        await _gameRepository.UpdateAsync(game);
    }

    public async Task<object> ProcessPaymentAsync(PaymentRequestDto paymentRequest)
    {
        var cart = await GetOpenCartAsync() ?? throw new GameServiceException("Cart is empty.");
        var cartItems = await _orderGameRepository.FindAsync(og => og.OrderId == cart.Id);
        if (!cartItems.Any())
        {
            throw new GameServiceException("Cart has no items.");
        }

        var totalSum = cartItems.Sum(i => i.Price * i.Quantity);
        var customerId = cart.CustomerId;
        var now = DateTime.UtcNow;

        try
        {
            switch (paymentRequest.Method)
            {
                case "Bank":
                    var stream = GeneratePdfInvoiceAsStream(customerId, cart.Id, now, now.AddDays(7), totalSum);
                    var fileName = $"invoice-{cart.Id}.pdf";
                    return (Stream: stream, FileName: fileName);

                case "IBox terminal":
                    var iboxPayload = new
                    {
                        transactionAmount = totalSum,
                        accountNumber = customerId,
                        invoiceNumber = cart.Id,
                    };

                    var iboxResponse = await _httpClient.PostAsJsonAsync("/api/payments/ibox", iboxPayload);

                    if (!iboxResponse.IsSuccessStatusCode)
                    {
                        var errorMsg = await iboxResponse.Content.ReadAsStringAsync();
                        throw new GameServiceException($"IBox payment failed: {iboxResponse.StatusCode} - {errorMsg}");
                    }

                    var iboxResult = await iboxResponse.Content.ReadFromJsonAsync<object>()
                        ?? throw new GameServiceException("Invalid response from IBox microservice.");

                    return iboxResult;

                case "Visa":
                    if (paymentRequest.Model == null)
                    {
                        throw new GameServiceException("Visa payment requires model data.");
                    }

                    var visaPayload = new
                    {
                        transactionAmount = totalSum,
                        cardHolderName = paymentRequest.Model.Holder,
                        cardNumber = paymentRequest.Model.CardNumber,
                        expirationMonth = paymentRequest.Model.MonthExpire,
                        expirationYear = paymentRequest.Model.YearExpire,
                        cvv = paymentRequest.Model.Cvv2,
                    };

                    var visaResponse = await _httpClient.PostAsJsonAsync("/api/payments/visa", visaPayload);

                    if (!visaResponse.IsSuccessStatusCode)
                    {
                        var visaError = await visaResponse.Content.ReadAsStringAsync();
                        throw new GameServiceException($"Visa payment failed: {visaResponse.StatusCode} - {visaError}");
                    }

                    var visaResult = await visaResponse.Content.ReadFromJsonAsync<object>()
                        ?? throw new GameServiceException("Invalid response from Visa microservice.");

                    return visaResult;

                default:
                    throw new GameServiceException("Unsupported payment method");
            }
        }
        catch (HttpRequestException ex)
        {
            throw new GameServiceException($"Payment service unreachable: {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            throw new GameServiceException("Payment request timed out.");
        }
        catch (Exception ex)
        {
            throw new GameServiceException($"Unexpected error during payment: {ex.Message}");
        }
    }

    private static Stream GeneratePdfInvoiceAsStream(Guid userId, Guid orderId, DateTime created, DateTime validUntil, double sum)
    {
        var stream = new MemoryStream();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Content()
                    .Column(column =>
                    {
                        column.Spacing(10);

                        column.Item().Text("Invoice").FontSize(20).Bold();
                        column.Item().Text($"User ID: {userId}");
                        column.Item().Text($"Order ID: {orderId}");
                        column.Item().Text($"Created: {created:yyyy-MM-dd HH:mm}");
                        column.Item().Text($"Valid Until: {validUntil:yyyy-MM-dd}");
                        column.Item().Text($"Total Sum: {sum} USD").Bold();
                    });
            });
        }).GeneratePdf(stream);

        stream.Position = 0;
        return stream;
    }

    // Helper methods
    private async Task<OrderEntity> GetOrCreateCartAsync()
    {
        // Using stub customer ID as per requirements
        var stubCustomerId = Guid.Parse("24967e32-dec1-47b5-8ca6-478afa84c2be");

        var cart = await GetOpenCartAsync();

        if (cart == null)
        {
            // Create new cart
            cart = new OrderEntity
            {
                Id = Guid.NewGuid(),
                CustomerId = stubCustomerId,
                Date = DateTime.UtcNow,
                Status = OrderStatus.Open,
            };

            await _orderRepository.AddAsync(cart);
        }

        return cart;
    }

    private async Task<OrderEntity> GetOpenCartAsync()
    {
        // Using stub customer ID as per requirements
        var stubCustomerId = Guid.Parse("24967e32-dec1-47b5-8ca6-478afa84c2be");

        var orders = await _orderRepository.FindAsync(o =>
            o.CustomerId == stubCustomerId && o.Status == OrderStatus.Open);
        return orders.FirstOrDefault();
    }
}
