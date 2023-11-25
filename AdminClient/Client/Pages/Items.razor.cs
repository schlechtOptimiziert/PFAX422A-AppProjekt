using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TransferModel;

namespace AdminClient.Client.Pages;

partial class Items : BasePage
{
    private IEnumerable<Item> items = Enumerable.Empty<Item>();

    protected override async Task OnInitializedAsync()
    {
        items = await LoadItemsAsync().ConfigureAwait(false);
    }

    private async Task<IEnumerable<Item>> LoadItemsAsync()
        => await Service.GetItemsAsync(CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Item>();

    private void GoToDetails(long id)
        => NavigationManager.NavigateTo($"/items/{id}");
}