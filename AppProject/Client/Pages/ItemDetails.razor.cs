using AppProject.Shared;
using Microsoft.AspNetCore.Components;

namespace AppProject.Client.Pages;

partial class ItemDetails : BasePage
{
    private Item item;

    [Parameter]
    public long? Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await base.OnInitializedAsync().ConfigureAwait(false);
        await LoadItemAsync().ConfigureAwait(false);
        IsLoading = false;
    }
    private async Task LoadItemAsync()
    {
        if (Id.HasValue)
            item = await Service.GetItemAsync(Id.Value, CancellationToken).ConfigureAwait(false) ?? throw new ArgumentException($"Could not find item with id '{Id}'");
    }
}
