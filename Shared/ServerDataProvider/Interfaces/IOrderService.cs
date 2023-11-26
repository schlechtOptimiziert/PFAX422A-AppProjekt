using TransferModel;

namespace ServerDataProvider.Interfaces;

public interface IOrderService
{
    public Task<long> AddOrderAsync(Order order, CancellationToken cancellationToken);
    public Task<IEnumerable<Order>> GetOrdersAsync(string userId, bool includeItems, CancellationToken cancellationToken);
    public Task<Order> GetOrderAsync(string userId, long orderId, CancellationToken cancellationToken);
}
