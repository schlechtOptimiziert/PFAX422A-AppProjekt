using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferModel;

namespace MobileClient.Client.Pages;

public partial class Homepage : BasePage
{
    private IEnumerable<Item> items = Enumerable.Empty<Item>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(false);
        items = await GetItemsAsync().ConfigureAwait(false);
    }

    private async Task<IEnumerable<Item>> GetItemsAsync()
        => await Service.GetItemsAsync(CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Item>();

    private void NavigateToList()
        => NavigationManager.NavigateTo($"/items");

    private void NavigateToList(Platform platform)
        => NavigationManager.NavigateTo($"/items?platform={platform.Id}");
}
