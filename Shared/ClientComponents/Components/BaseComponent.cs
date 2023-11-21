using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ServerDataProvider.Interfaces;

namespace ClientComponents.Components;

public class BaseComponent : ComponentBase
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    protected CancellationToken CancellationToken { get; }

    [Inject]
    public IService Service { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject]
    public ToastService ToastService { get; set; }

    public bool IsLoading { get; set; }
    public bool IsCreate { get; set; }
    public string UserId { get; private set; }

    protected BaseComponent() => CancellationToken = cancellationTokenSource.Token;

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(false);
        var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        if (authenticationState != null)
        {
            var user = authenticationState.User;
            UserId = user.Claims.SingleOrDefault(c => c.Type == "sub")?.Value;
        }
    }
}

