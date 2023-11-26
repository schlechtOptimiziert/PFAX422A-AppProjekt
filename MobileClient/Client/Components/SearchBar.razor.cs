using ClientComponents.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using TransferModel;

namespace MobileClient.Client.Components;

public partial class SearchBar : BaseComponent
{
    private string searchText;

    [Parameter]
    public string Class { get; set; } = default!;

    [Parameter]
    public string Style { get; set; } = default!;

    public void KeyPressed(KeyboardEventArgs e)
    {
        if (e.Code == "Enter" || e.Code == "NumpadEnter")
            NavigateToList(searchText);
    }

    private void NavigateToList(string searchText)
    {
        int? platformId = null;
        if (NavigationManager.TryGetQueryString<int>("platform", out var platformIfFromQuery))
            platformId = platformIfFromQuery;

        if (string.IsNullOrEmpty(searchText))
        {
            if (platformId == null)
                NavigationManager.NavigateTo($"/items");
            else
                NavigationManager.NavigateTo($"/items?platform={platformId}");
        }
        else
        {
            if (platformId == null)
                NavigationManager.NavigateTo($"/items?searchText={searchText}");
            else
                NavigationManager.NavigateTo($"/items?platform={platformId}&searchText={searchText}");
        }
    }
}
