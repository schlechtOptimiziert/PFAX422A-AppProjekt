using AppProject.Client.Helpers;
using AppProject.Client.Interfaces;
using AppProject.Shared;

namespace AppProject.Client;

public partial class Service : IItemService
{
    private readonly string itemRequestUri = "api/Items";

    public async Task<long> AddItemAsync(Item item, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync($"{itemRequestUri}", item, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<long>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<Item> GetItemAsync(long itemId, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsync($"{itemRequestUri}/{itemId}", null, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<Item>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Item>> GetItemsAsync(long campaignId, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsync($"{itemRequestUri}?campaignId={campaignId}", null, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<IEnumerable<Item>>(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateItemAsync(Item item, CancellationToken cancellationToken)
    {
        var response = await httpClient.PutAsJsonAsync($"{itemRequestUri}", item, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteItemAsync(long id, CancellationToken cancellationToken)
    {
        var response = await httpClient.DeleteAsync($"{itemRequestUri}/{id}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }
}
