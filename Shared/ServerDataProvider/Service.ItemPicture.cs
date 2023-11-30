using ServerDataProvider.Helpers;
using ServerDataProvider.Interfaces;
using System.Threading;
using TransferModel;

namespace ServerDataProvider;

public partial class Service : IItemPictureService
{
    public async Task<long> AddItemPictureAsync(ItemPicture picture, long itemId, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync($"{itemRequestUri}/{itemId}/Pictures", picture, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<long>(cancellationToken).ConfigureAwait(false);
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

    public async Task<IEnumerable<string>> UploadFiles(MultipartFormDataContent content, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsync($"/api/File", content, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<IEnumerable<string>>(cancellationToken).ConfigureAwait(false);
    }
}
