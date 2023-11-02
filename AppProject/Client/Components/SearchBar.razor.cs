using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AppProject.Client.Components;

public partial class SearchBar : BaseComponent
{
    private string? searchText;
    private Platform? platform;

    [Parameter]
    public string? Class { get; set; } = default!;

    [Parameter]
    public string? Style { get; set; } = default!;

    public void KeyPressed(KeyboardEventArgs e)
    {
        if (e.Code == "Enter" || e.Code == "NumpadEnter")
        {
            NavigateToList(searchText);
        }
    }

    private void NavigateToList(string? searchText)
    {
        platform = PlatformExtensions.GetPlatformFromUri(NavigationManager);
        if (!string.IsNullOrEmpty(searchText))
            if (platform == null)
                NavigationManager.NavigateTo($"/items?searchText={searchText}");
            else
                NavigationManager.NavigateTo($"/items?platform={platform}&searchText={searchText}");
    }
}