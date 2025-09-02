using AdminPanel.Entity;

namespace AdminPanel.Dal.Repositories;
public interface IOrderRepository
{
    Task AddGameToCartAsync(Guid customerId, Guid productId, int quantity, double price);

    Task UpdateGameQuantityInCartAsync(Guid customerId, Guid productId, int quantity);

    Task DeleteGameFromCartAsync(Guid customerId, Guid productId);

    Task<IEnumerable<OrderEntity>> GetPaidAndCancelledOrdersAsync();

    Task<OrderEntity> GetOrderByIdAsync(Guid orderId);

    Task<OrderEntity> GetOrderDetailsAsync(Guid orderId);

    Task<IEnumerable<OrderGameEntity>> GetOrderGamesAsync(Guid orderId);

    Task ProcessPaymentAsync(Guid orderId, string paymentMethod);
}
