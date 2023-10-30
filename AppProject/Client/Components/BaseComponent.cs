using AppProject.ServerDataProvider.Interfaces;
using Microsoft.AspNetCore.Components;

namespace AppProject.Client.Components;

public abstract class BaseComponent : ComponentBase
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    protected CancellationToken CancellationToken { get; }

    [Inject]
    public IService Service { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public bool IsLoading { get; set; }
    public bool IsCreate { get; set; }

    protected BaseComponent() => CancellationToken = cancellationTokenSource.Token;
}
