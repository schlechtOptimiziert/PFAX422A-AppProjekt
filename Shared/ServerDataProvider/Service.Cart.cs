﻿using ServerDataProvider.Helpers;
using ServerDataProvider.Interfaces;
using TransferModel;

namespace ServerDataProvider;

public partial class Service : IItemService
{
    private readonly string cartRequestUri = "api/Carts";

    public async Task AddItemToCartAsync(string userId, long itemId, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync($"{cartRequestUri}/{userId}", itemId, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<CartItemLink>> GetCartItemLinksAsync(string userId, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"{cartRequestUri}/{userId}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsAsync<IEnumerable<CartItemLink>>(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateItemCartAmountAsync(string userId, CartItemLink cartItemLink, CancellationToken cancellationToken)
    {
        var response = await httpClient.PutAsJsonAsync($"{cartRequestUri}/{userId}", cartItemLink, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteItemAsync(string userId, long itemId, CancellationToken cancellationToken)
    {
        var response = await httpClient.DeleteAsync($"{cartRequestUri}/{userId}/{itemId}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }
}