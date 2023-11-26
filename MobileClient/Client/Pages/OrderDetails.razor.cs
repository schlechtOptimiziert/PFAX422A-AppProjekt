using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TransferModel;

namespace MobileClient.Client.Pages;

public partial class OrderDetails : BasePage
{
    private Order order;

    [Parameter]
    public long? Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await base.OnInitializedAsync().ConfigureAwait(false);
        order = await GetItemAsync().ConfigureAwait(false);
        IsLoading = false;
    }

    private async Task<Order> GetItemAsync()
    {
        if (Id.HasValue)
            return await Service.GetOrderAsync(UserId, Id.Value, CancellationToken).ConfigureAwait(false) ?? null;
        return null;
    }
}