using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MudBlazor;
using TransferModel;

namespace MobileClient.Client.Pages;

public partial class PaymentPage : BasePage
{
    private MudForm form;
    private ICollection<CartItemLink> cartItems = new List<CartItemLink>();
    private Order order = new();

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

    private async Task CreateOrderAsync(CancellationToken cancellationToken)
    {
        await form.Validate();
        if (!form.IsValid)
            return;
        order.UserId = UserId;
        order.Date = DateTime.Now;
        var orderId = await Service.AddOrderAsync(order, cancellationToken).ConfigureAwait(false);
        NavigationManager.NavigateTo($"/Orders/{orderId}");
    }
}