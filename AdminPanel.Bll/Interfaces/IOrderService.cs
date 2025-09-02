using AdminPanel.Bll.DTOs;

namespace AdminPanel.Bll.Interfaces;
public interface IOrderService
{
    // US1 - Cart management
    Task AddGameToCartAsync(Guid gameId);

    Task RemoveGameFromCartAsync(string key);

    // US2 - Order retrieval
    Task<IEnumerable<OrderDto>> GetOrdersAsync();

    Task<OrderDto> GetOrderByIdAsync(Guid orderId);

    Task<IEnumerable<OrderGameDto>> GetCartAsync();

    Task<IEnumerable<OrderGameDto>> GetOrderDetailsAsync(Guid orderId);

    Task<PaymentMethodsDto> GetPaymentMethodsAsync();

    Task<object> ProcessPaymentAsync(PaymentRequestDto paymentRequest);

    // US17-20 - Order management
    Task UpdateOrderDetailQuantityAsync(Guid detailId, int quantity);

    Task DeleteOrderDetailAsync(Guid detailId);

    Task ShipOrderAsync(Guid orderId);

    Task AddGameToOrderAsync(Guid orderId, string gameKey);
}
