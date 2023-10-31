using AppProject.Client.Components;
using AppProject.Shared;
using Microsoft.AspNetCore.Components.Routing;

namespace AppProject.Client.Pages
{
    public partial class ProductListPage : BasePage
    {
        private Platform? platform;
        private IEnumerable<Item> items = Enumerable.Empty<Item>();
        private string? searchText;

        protected override async Task OnInitializedAsync()
        {
            await LoadItemsAsync().ConfigureAwait(false);
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

        private async Task LoadItemsAsync()
        {
            items = await Service.GetItemsAsync(CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Item>();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (searchText != null)
            {
                items = items.Where(i => i.Name.ToLower().Contains(searchText.ToLower()));
            }
        }

        public void Dispose() => NavigationManager.LocationChanged -= HandleLocationChanged;
    }
}