using Microsoft.AspNetCore.Components.Routing;
using MobileClient.Client.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferModel;

namespace MobileClient.Client.Pages;

public partial class ProductListPage : BasePage
{
    private Platform? platform;
    private IEnumerable<Item> items = Enumerable.Empty<Item>();
    private IEnumerable<Item> filteredItems = Enumerable.Empty<Item>();
    private string? searchText;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(false);
        items = await GetItemsAsync().ConfigureAwait(false);
        filteredItems = items;
        GetFiltersFromQuery();
        NavigationManager.LocationChanged += HandleLocationChanged;
    }

    private void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        GetFiltersFromQuery();
        StateHasChanged();
    }

    private void GetFiltersFromQuery()
    {
        platform = PlatformExtensions.GetPlatformFromUri(NavigationManager);
        NavigationManager.TryGetQueryString("searchText", out searchText);
        ApplyFilters();
    }

    private async Task<IEnumerable<Item>> GetItemsAsync()
        => await Service.GetItemsAsync(CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Item>();

    private void ApplyFilters()
    {
        if (searchText != null)
            filteredItems = items.Where(i => i.Name.ToLower().Contains(searchText.ToLower()));
        else
            filteredItems = items;
    }

    private void NavigateToList(long id)
        => NavigationManager.NavigateTo($"/items/{id}");

    public void Dispose() => NavigationManager.LocationChanged -= HandleLocationChanged;
}
