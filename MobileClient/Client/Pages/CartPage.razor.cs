using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TransferModel;

namespace MobileClient.Client.Pages
{
    public partial class CartPage : BasePage
    {
        private ICollection<CartItemLink> cartItems = new List<CartItemLink>();

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authenticationState != null)
            {
                var user = authenticationState.User;
                var userId = user.Claims.SingleOrDefault(c => c.Type == "sub")?.Value;
                cartItems = (await GetCartItemLinksAsync(userId, CancellationToken).ConfigureAwait(false)).ToList();
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);

            foreach (var item in cartItems.Select(c => c.Item))
            {
                var coverPicture = await Service.GetItemCoverPictureAsync(item.Id, CancellationToken).ConfigureAwait(false);
                item.CoverPictureUri = ItemPicture.ItemPictureToUri(coverPicture);
            }
        }

        private async Task<IEnumerable<CartItemLink>> GetCartItemLinksAsync(string userId, CancellationToken cancellationToken)
            => await Service.GetCartItemLinksAsync(userId, cancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<CartItemLink>();

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
    }
}