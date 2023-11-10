using Microsoft.AspNetCore.Components;
using Shared.ServerDataProvider.Interfaces;
using Shared.SharedClientComponents.Toast;

namespace AdminClient;

public abstract class BasePage : ComponentBase
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    protected CancellationToken CancellationToken { get; }

    [Inject]
    public IService Service { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    [Inject]
    public ToastService ToastService { get; set; }

    public bool IsLoading { get; set; }
    public bool IsCreate { get; set; }

    protected BasePage() => CancellationToken = cancellationTokenSource.Token;
}
