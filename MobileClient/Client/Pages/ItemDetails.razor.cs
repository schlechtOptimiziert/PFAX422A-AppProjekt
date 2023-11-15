﻿using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferModel;

namespace MobileClient.Client.Pages;

partial class ItemDetails : BasePage
{
    private Item item;

    private IEnumerable<string> pictures = Enumerable.Empty<string>();

    [Parameter]
    public long? Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await base.OnInitializedAsync().ConfigureAwait(false);
        item = await GetItemAsync().ConfigureAwait(false);
        pictures = (await GetItemPicturesAsync().ConfigureAwait(false))?.Select(ItemPicture.ItemPictureToUri);
        IsLoading = false;
    }

    private async Task<Item> GetItemAsync()
    {
        if (Id.HasValue)
            return await Service.GetItemAsync(Id.Value, CancellationToken).ConfigureAwait(false) ?? null;
        return null;
    }

    private async Task<IEnumerable<ItemPicture>> GetItemPicturesAsync()
    {
        if (Id.HasValue)
            return await Service.GetItemPicturesAsync(Id.Value, CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<ItemPicture>();
        return Enumerable.Empty<ItemPicture>();
    }
}