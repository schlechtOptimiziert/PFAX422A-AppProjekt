using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TransferModel;

namespace MobileClient.Client.Pages;

public partial class Homepage : BasePage
{
    private IEnumerable<Item> items = Enumerable.Empty<Item>();
    private IEnumerable<Item> buyAgainItems = Enumerable.Empty<Item>();
    private IEnumerable<Platform> platforms = Enumerable.Empty<Platform>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(false);
        items = await GetItemsAsync(CancellationToken).ConfigureAwait(false);
        buyAgainItems = await GetBuyAgainItemsAsync(CancellationToken).ConfigureAwait(false);
        platforms = await GetPlatformsAsync(CancellationToken).ConfigureAwait(false);
    }

    private async Task<IEnumerable<Item>> GetItemsAsync(CancellationToken cancellationToken)
        => await Service.GetItemsAsync(cancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Item>();

    private async Task<IEnumerable<Item>> GetBuyAgainItemsAsync(CancellationToken cancellationToken)
    {
        var orders = await Service.GetOrdersAsync(UserId, true, cancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Order>();
        List<Item> orderItems = new();
        foreach (var order in orders)
            if (orderItems.Count < 10)
                orderItems = orderItems.Concat(order.Items.DistinctBy(o => o.Id)).ToList();
        return orderItems;
    }

    private async Task<IEnumerable<Platform>> GetPlatformsAsync(CancellationToken cancellationToken)
        => await Service.GetPlatformsAsync(cancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Platform>();

    private void NavigateToList()
        => NavigationManager.NavigateTo($"/items");

    private void NavigateToList(Platform platform)
        => NavigationManager.NavigateTo($"/items?platform={platform.Id}");
}
