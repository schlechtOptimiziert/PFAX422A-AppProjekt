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
        item = await GetItemAsync().ConfigureAwait(false);
        IsLoading = false;
    }

    private async Task<Item> GetItemAsync()
    {
        if (Id.HasValue)
            return await Service.GetItemAsync(Id.Value, CancellationToken).ConfigureAwait(false) ?? null;
        return null;
    }
}
