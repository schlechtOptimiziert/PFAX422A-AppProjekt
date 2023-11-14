using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using ServerDataProvider.Interfaces;
using System.Threading;

namespace MobileClient.Client.Components;

public abstract class BaseComponent : ComponentBase
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    protected CancellationToken CancellationToken { get; }

    [Inject]
    public IService Service { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public bool IsLoading { get; set; }
    public bool IsCreate { get; set; }

    protected BaseComponent() => CancellationToken = cancellationTokenSource.Token;
}
