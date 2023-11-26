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
    private string name;
    private string street;
    private string streetNumber;
    private int? postcode;
    private string city;
    private string country;

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
        => string.IsNullOrEmpty(name)
        || string.IsNullOrEmpty(street)
        || string.IsNullOrEmpty(streetNumber)
        || postcode == null
        || string.IsNullOrEmpty(city)
        || string.IsNullOrEmpty(country);

    private async Task CreateOrderAsync(CancellationToken cancellationToken)
    {
        if (PayDisabled()) return;
        var order = new Order
        {
            UserId = UserId,
            Date = DateTime.Now,
            Name = name,
            Street = street,
            StreetNumber = streetNumber,
            Postcode = postcode.Value,
            City = city,
            Country = country
        };
        var orderId = await Service.AddOrderAsync(order, cancellationToken).ConfigureAwait(false);
        NavigationManager.NavigateTo($"/Orders/{orderId}");
    }
}