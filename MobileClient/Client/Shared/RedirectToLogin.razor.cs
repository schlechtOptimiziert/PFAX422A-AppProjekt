using ClientComponents.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace MobileClient.Client.Shared;

public partial class RedirectToLogin : BaseComponent
{
    [Inject]
    private NavigationManager Navigation { get; set; }

    protected override void OnInitialized()
    {
        Navigation.NavigateToLogin("authentication/login");
    }
}
