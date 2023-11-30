using ServerDataProvider.Helpers;
using ServerDataProvider.Interfaces;
using System.Threading;
using TransferModel;

namespace ServerDataProvider;

public partial class Service : IItemPictureService
{
    public async Task<IEnumerable<long>> AddItemPicturesAsync(MultipartFormDataContent pictures, long itemId, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsync($"{itemRequestUri}/{itemId}/Pictures", pictures, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<IEnumerable<long>>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"{itemRequestUri}/{itemId}/Pictures", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<IEnumerable<ItemPicture>>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<ItemPicture> GetItemCoverPictureAsync(long itemId, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"{itemRequestUri}/{itemId}/Pictures/CoverPicture", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<ItemPicture>(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteItemPictureAsync(long itemId, long id, CancellationToken cancellationToken)
    {
        var response = await httpClient.DeleteAsync($"{itemRequestUri}/{itemId}/Pictures/{id}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }
}
