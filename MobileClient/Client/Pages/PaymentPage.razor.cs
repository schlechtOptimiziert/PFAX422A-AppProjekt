using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TransferModel;

namespace MobileClient.Client.Pages;

public partial class PaymentPage : BasePage
{
    private ICollection<CartItemLink> cartItems = new List<CartItemLink>();
    private Order order;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(false);
        if (!string.IsNullOrEmpty(UserId))
            cartItems = await GetCartItemLinksAsync(UserId, CancellationToken).ConfigureAwait(false);
    }

    private async Task<ICollection<CartItemLink>> GetCartItemLinksAsync(string userId, CancellationToken cancellationToken)
        => (await Service.GetCartItemLinksAsync(userId, cancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<CartItemLink>()).ToList();

    private decimal GetTotal()
        => cartItems.Sum(x => x.Amount * x.Item.Price);

    private bool PayDisabled()
        => string.IsNullOrEmpty(order.Name)
        || string.IsNullOrEmpty(order.Street)
        || string.IsNullOrEmpty(order.StreetNumber)
        || order.Postcode == null
        || string.IsNullOrEmpty(order.City)
        || string.IsNullOrEmpty(order.Country);

    private async Task CreateOrderAsync(CancellationToken cancellationToken)
    {
        if (PayDisabled())
            return;
        order.UserId = UserId;
        order.Date = DateTime.Now;
        var orderId = await Service.AddOrderAsync(order, cancellationToken).ConfigureAwait(false);
        NavigationManager.NavigateTo($"/Orders/{orderId}");
    }
}