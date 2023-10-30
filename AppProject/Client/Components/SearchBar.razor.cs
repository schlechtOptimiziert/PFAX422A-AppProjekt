using Microsoft.AspNetCore.Components;

namespace AppProject.Client.Components
{
    public partial class SearchBar : BaseComponent
    {
        private string? searchText;

        [Parameter]
        public string? Class { get; set; } = default!;

        [Parameter]
        public string? Style { get; set; } = default!;

        private void NavigateToList(Platform platform, string? searchText)
        {
            if (!string.IsNullOrEmpty(searchText))
                NavigationManager.NavigateTo($"/list/{platform}/{searchText}");
        }
    }
}