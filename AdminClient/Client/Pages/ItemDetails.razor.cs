using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferModel;

namespace AdminClient.Client.Pages;

partial class ItemDetails : BasePage
{
    MudForm form;
    Color badgeColor = Color.Success;
    string badgeIcon = Icons.Material.Outlined.Lock;

    private Item item = new();
    private IEnumerable<ItemPicture> pictures = Enumerable.Empty<ItemPicture>();


    [Parameter]
    public long? Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await base.OnInitializedAsync().ConfigureAwait(false);

        if (Id == null)
            IsCreate = true;

        if (!IsCreate)
        {
            item = await GetItemAsync().ConfigureAwait(false);
            pictures = await GetItemPicturesAsync().ConfigureAwait(false);
        }

        IsLoading = false;
    }

    private async Task SaveItem()
    {
        await form.Validate().ConfigureAwait(false);
        if (!form.IsValid)
            return;

        IsLoading = true;
        if (IsCreate)
        {
            var itemId = await AddItemAsync().ConfigureAwait(false);
            NavigationManager.NavigateTo($"/items/{itemId}", true);
            return;
        }
        else
        {
            await UpdateItem().ConfigureAwait(false);
        }

        item = await GetItemAsync().ConfigureAwait(false);
        badgeColor = Color.Success;
        badgeIcon = Icons.Material.Outlined.Lock;
        IsLoading = false;
    }
    private async Task UpdateItem()
        => await Service.UpdateItemAsync(item, CancellationToken).ConfigureAwait(false);

    private async Task<long> AddItemAsync()
        => await Service.AddItemAsync(item, CancellationToken).ConfigureAwait(false);

    private async Task<Item> GetItemAsync()
    {
        if (Id.HasValue)
            return await Service.GetItemAsync(Id.Value, CancellationToken).ConfigureAwait(false) ?? throw new ArgumentException($"Could not find item with id '{Id}'");
        return null;
    }

    private async Task<IEnumerable<ItemPicture>> GetItemPicturesAsync()
    {
        if (Id.HasValue)
            return await Service.GetItemPicturesAsync(Id.Value, CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<ItemPicture>();
        return Enumerable.Empty<ItemPicture>();
    }

    private void FieldChanged()
    {
        badgeColor = Color.Error;
        badgeIcon = Icons.Material.Filled.LockOpen;
    }

    private async Task UploadPicture(IBrowserFile file)
    {
        if (Id.HasValue)
        {
            var itemPicture = await ItemPicture.BrowserFileToItemPictureAsync(file, Id.Value, CancellationToken).ConfigureAwait(false);
            await Service.AddItemPictureAsync(itemPicture, Id.Value, CancellationToken).ConfigureAwait(false);
        }
        pictures = await GetItemPicturesAsync().ConfigureAwait(false);
    }

    private async Task DeleteItemPicture(ItemPicture itemPicture)
    {
        await Service.DeleteItemPictureAsync(itemPicture.ItemId, itemPicture.Id, CancellationToken).ConfigureAwait(false);
        pictures = await GetItemPicturesAsync().ConfigureAwait(false);
    }
}