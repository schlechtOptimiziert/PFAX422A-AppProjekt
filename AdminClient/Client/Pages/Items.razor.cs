using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferModel;

namespace AdminClient.Client.Pages;

partial class Items : BasePage
{
    private IEnumerable<Item> items = Enumerable.Empty<Item>();

    [Parameter]
    public long? Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadItemsAsync().ConfigureAwait(false);
    }

    private async Task LoadItemsAsync()
        => items = await Service.GetItemsAsync(CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Item>();

    private void GoToDetails(long id)
        => NavigationManager.NavigateTo($"/items/{id}");
}