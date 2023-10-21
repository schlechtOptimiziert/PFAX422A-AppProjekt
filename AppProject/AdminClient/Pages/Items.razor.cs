using AppProject.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AppProject.AdminClient.Pages;

partial class Items : BasePage
{
    MudForm form;
    Color badgeColor = Color.Success;
    string badgeIcon = Icons.Material.Outlined.Lock;

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
