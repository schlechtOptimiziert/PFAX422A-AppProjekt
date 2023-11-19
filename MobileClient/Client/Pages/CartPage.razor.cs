using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TransferModel;

namespace MobileClient.Client.Pages
{
    public partial class CartPage : BasePage
    {
        private IEnumerable<CartItemLink> cartItems = Enumerable.Empty<CartItemLink>();

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
                cartItems = await GetCartItemLinksAsync(userId).ConfigureAwait(false);
            }
        }

        private async Task<IEnumerable<CartItemLink>> GetCartItemLinksAsync(string userId)
            => await Service.GetCartItemLinksAsync(userId, CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<CartItemLink>();
    }
}