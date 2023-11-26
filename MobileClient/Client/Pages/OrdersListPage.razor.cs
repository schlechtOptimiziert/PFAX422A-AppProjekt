using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferModel;

namespace MobileClient.Client.Pages;

public partial class OrdersListPage : BasePage
{
    private IEnumerable<Order> orders = Enumerable.Empty<Order>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(false);
        orders = await GetOrdersAsync().ConfigureAwait(false);
    }

    private async Task<IEnumerable<Order>> GetOrdersAsync()
        => await Service.GetOrdersAsync(UserId, false, CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Order>();

    private void NavigateToOrder(long id)
        => NavigationManager.NavigateTo($"/Orders/{id}");
}