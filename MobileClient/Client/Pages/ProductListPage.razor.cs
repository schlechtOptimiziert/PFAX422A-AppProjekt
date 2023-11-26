using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using TransferModel;

namespace MobileClient.Client.Pages;

public partial class ProductListPage : BasePage
{
    private long? platformId;
    private IEnumerable<Item> items = Enumerable.Empty<Item>();
    private IEnumerable<Item> filteredItems = Enumerable.Empty<Item>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(false);
        platformId = GetPlatformIdFromQuery();
        items = await GetItemsAsync().ConfigureAwait(false);
        NavigationManager.LocationChanged += HandleLocationChanged;
        ApplyFilters();
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync().ConfigureAwait(false);

        foreach (var item in filteredItems)
        {
            var coverPicture = await Service.GetItemCoverPictureAsync(item.Id, CancellationToken).ConfigureAwait(false);

            if (coverPicture != null)
                item.CoverPictureUri = ItemPicture.ItemPictureToUri(coverPicture);
        }
    }

    private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
    {
        ApplyFilters();
        StateHasChanged();
    }

    private long? GetPlatformIdFromQuery()
    {
        if (NavigationManager.TryGetQueryString("platform", out string platformString))
            return long.Parse(platformString);
        else
            return null;
    }

    private async Task<IEnumerable<Item>> GetItemsAsync()
    {
        if (platformId == null)
            return await Service.GetItemsAsync(CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Item>();
        else
            return await Service.GetPlatformFilteredItemsAsync(platformId.Value, CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Item>();
    }

    private void ApplyFilters()
    {
        if (NavigationManager.TryGetQueryString("searchText", out string searchText))
            filteredItems = items.Where(i => i.Name.ToLower().Contains(searchText.ToLower()));
        else
            filteredItems = items;
    }

    private void NavigateToList(long id)
        => NavigationManager.NavigateTo($"/items/{id}");

    public void Dispose() => NavigationManager.LocationChanged -= HandleLocationChanged;
}
