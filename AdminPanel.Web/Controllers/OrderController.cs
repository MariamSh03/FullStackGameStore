using AdminPanel.Bll.Constants;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Web.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // US2 - Get all orders
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        try
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // US2 - Get specific order by ID
    [HttpGet("{id}")]
    [RequirePermission(Permissions.ViewOrders)]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        try
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // US3 - Get order details (games inside the order)
    [HttpGet("{id}/details")]
    [RequirePermission(Permissions.ViewOrders)]
    public async Task<IActionResult> GetOrderDetails(Guid id)
    {
        try
        {
            var details = await _orderService.GetOrderDetailsAsync(id);
            return Ok(details);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // US4 - Get current user's cart
    [HttpGet("cart")]
    public async Task<IActionResult> GetCart()
    {
        try
        {
            var cart = await _orderService.GetCartAsync();
            return Ok(cart);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // US1 - Add game to cart
    [HttpPost("cart/{gameId}")]
    [Authorize] // Any authenticated user can add to cart
    public async Task<IActionResult> AddGameToCart(Guid gameId)
    {
        try
        {
            await _orderService.AddGameToCartAsync(gameId);
            return Ok(new { Message = "Game added to cart." });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // US1 - Remove game from cart
    [HttpDelete("cart/{key}")]
    [Authorize] // Any authenticated user can manage their cart
    public async Task<IActionResult> RemoveGameFromCart(string key)
    {
        try
        {
            await _orderService.RemoveGameFromCartAsync(key);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // US5 - Get available payment methods
    [HttpGet("payment-methods")]
    [Authorize] // Any authenticated user can see payment methods
    public async Task<IActionResult> GetPaymentMethods()
    {
        try
        {
            var methods = await _orderService.GetPaymentMethodsAsync();
            return Ok(methods);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // US6-8 - Process payment
    [HttpPost("payment")]
    [Authorize] // Any authenticated user can make payments
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto request)
    {
        try
        {
            var result = await _orderService.ProcessPaymentAsync(request);

            return result is ValueTuple<Stream, string> pdfResult ? File(pdfResult.Item1, "application/pdf", pdfResult.Item2) : Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // US17 - Update order detail quantity
    [HttpPatch("details/{id}/quantity")]
    [RequirePermission(Permissions.EditOrders)]
    public async Task<IActionResult> UpdateOrderDetailQuantity(Guid id, [FromBody] UpdateQuantityRequestDto request)
    {
        try
        {
            await _orderService.UpdateOrderDetailQuantityAsync(id, request.Count);
            return Ok(new { Message = "Order detail quantity updated successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // US18 - Delete order detail
    [HttpDelete("details/{id}")]
    [RequirePermission(Permissions.EditOrders)]
    public async Task<IActionResult> DeleteOrderDetail(Guid id)
    {
        try
        {
            await _orderService.DeleteOrderDetailAsync(id);
            return Ok(new { Message = "Order details delated successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // US19 - Ship order
    [HttpPost("{id}/ship")]
    [RequirePermission(Permissions.ShipOrders)]
    public async Task<IActionResult> ShipOrder(Guid id)
    {
        try
        {
            await _orderService.ShipOrderAsync(id);
            return Ok(new { Message = "Order shipped successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // US20 - Add game as order detail
    [HttpPost("{id}/details/{key}")]
    [RequirePermission(Permissions.EditOrders)]
    public async Task<IActionResult> AddGameToOrder(Guid id, string key)
    {
        try
        {
            await _orderService.AddGameToOrderAsync(id, key);
            return Ok(new { Message = "Game added to order successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
