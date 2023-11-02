using AppProject.ServerDataProvider.Helpers;
using AppProject.ServerDataProvider.Interfaces;
using AppProject.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppProject.ServerDataProvider;

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
}
