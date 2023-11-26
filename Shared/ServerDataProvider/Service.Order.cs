using ServerDataProvider.Helpers;
using ServerDataProvider.Interfaces;
using TransferModel;

namespace ServerDataProvider;

public partial class Service : IOrderService
{
    private readonly string orderRequestUri = "api/Users/{0}/Orders";

    public async Task<long> AddOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync($"{string.Format(orderRequestUri, order.UserId)}", order, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<long>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<Order> GetOrderAsync(string userId, long orderId, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"{string.Format(orderRequestUri, userId)}/{orderId}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<Order>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync(string userId, bool includeItems, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"{string.Format(orderRequestUri, userId)}?includePositions={includeItems}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<IEnumerable<Order>>(cancellationToken).ConfigureAwait(false);
    }
}
