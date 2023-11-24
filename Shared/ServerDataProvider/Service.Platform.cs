using ServerDataProvider.Helpers;
using ServerDataProvider.Interfaces;
using TransferModel;

namespace ServerDataProvider;

public partial class Service : IPlatformService
{
    private readonly string platformRequestUri = "api/Platforms";

    public async Task<long> AddPlatformAsync(Platform platform, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync($"{platformRequestUri}", platform, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<long>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<Platform> GetPlatformAsync(long platformId, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"{platformRequestUri}/{platformId}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<Platform>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Platform>> GetPlatformsAsync(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"{platformRequestUri}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<IEnumerable<Platform>>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Item>> GetPlatformFilteredItemsAsync(long platformId, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"{platformRequestUri}/{platformId}/Items", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<IEnumerable<Item>>(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdatePlatformAsync(Platform platform, CancellationToken cancellationToken)
    {
        var response = await httpClient.PutAsJsonAsync($"{platformRequestUri}", platform, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeletePlatformAsync(long platformId, CancellationToken cancellationToken)
    {
        var response = await httpClient.DeleteAsync($"{platformRequestUri}/{platformId}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }
}
