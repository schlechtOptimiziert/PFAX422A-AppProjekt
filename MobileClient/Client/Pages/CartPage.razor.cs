using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TransferModel;

namespace MobileClient.Client.Pages;

public partial class CartPage : BasePage
{
    private ICollection<CartItemLink> cartItems = new List<CartItemLink>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(false);
        if (!string.IsNullOrEmpty(UserId))
            cartItems = await GetCartItemLinksAsync(UserId, CancellationToken).ConfigureAwait(false);
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync().ConfigureAwait(false);

        foreach (var item in cartItems.Select(c => c.Item))
        {
            var coverPicture = await Service.GetItemCoverPictureAsync(item.Id, CancellationToken).ConfigureAwait(false);

            if (coverPicture != null)
                item.CoverPicturePath = $"/Images/{coverPicture.FileName}";
        }
    }

    private async Task<ICollection<CartItemLink>> GetCartItemLinksAsync(string userId, CancellationToken cancellationToken)
        => (await Service.GetCartItemLinksAsync(userId, cancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<CartItemLink>()).ToList();

    private decimal GetTotal()
        => cartItems.Sum(x => x.Amount * x.Item.Price);

    private async Task DeleteCartItemAsync(CartItemLink cartItem, CancellationToken cancellationToken)
    {
        await Service.DeleteItemFromCartAsync(cartItem.UserId, cartItem.ItemId, cancellationToken).ConfigureAwait(false);
        cartItems.Remove(cartItem);
        StateHasChanged();
    }

    private async Task UpdateCartItemAmountAsync(int newAmount, CartItemLink cartItem, CancellationToken cancellationToken)
    {
        cartItem.Amount = newAmount;
        await Service.UpdateItemCartAmountAsync(cartItem, cancellationToken).ConfigureAwait(false);
    }

    private void NavigateToPayment()
        => NavigationManager.NavigateTo("/cart/payment");
}