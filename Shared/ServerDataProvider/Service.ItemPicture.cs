﻿using ServerDataProvider.Helpers;
using ServerDataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

    public async Task DeleteItemPictureAsync(long itemId, long id, CancellationToken cancellationToken)
    {
        var response = await httpClient.DeleteAsync($"{itemRequestUri}/{itemId}/Pictures/{id}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }
}