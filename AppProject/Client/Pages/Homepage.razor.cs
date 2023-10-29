﻿using AppProject.Client.Pages.Components;
using AppProject.Shared;

namespace AppProject.Client.Pages
{
    public partial class Homepage : BasePage
    {
        private string? searchText;
        private IEnumerable<Item> items = Enumerable.Empty<Item>();

        protected override async Task OnInitializedAsync()
        {
            await LoadItemsAsync().ConfigureAwait(false);
        }
        private async Task LoadItemsAsync()
            => items = await Service.GetItemsAsync(CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Item>();

        private void NavigateToList(Platform platform)
        {
            NavigationManager.NavigateTo($"/list/{platform}");
        }
        private void NavigateToList(Platform platform, string? searchText)
        {
            if (!string.IsNullOrEmpty(searchText))
                NavigationManager.NavigateTo($"/list/{platform}/{searchText}");
        }
    }
}