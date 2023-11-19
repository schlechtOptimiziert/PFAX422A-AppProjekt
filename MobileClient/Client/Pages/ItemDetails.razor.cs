using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TransferModel;

namespace MobileClient.Client.Pages;

partial class ItemDetails : BasePage
{
    private Item item;
    private string userId;
    private IEnumerable<string> pictures = Enumerable.Empty<string>();

    [Parameter]
    public long? Id { get; set; }

    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await base.OnInitializedAsync().ConfigureAwait(false);
        item = await GetItemAsync().ConfigureAwait(false);
        pictures = (await GetItemPicturesAsync().ConfigureAwait(false))?.Select(ItemPicture.ItemPictureToUri);
        var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        if (authenticationState != null)
        {
            var user = authenticationState.User;
            userId = user.Claims.SingleOrDefault(c => c.Type == "sub")?.Value;
        }
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

    private async Task AddItemToCartAsync(string userId, long itemId, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(userId))
            await Service.AddItemToCartAsync(userId, itemId, cancellationToken);
    }
}
