﻿using AppProject.ServerDataProvider.Interfaces;
using Microsoft.AspNetCore.Components;

namespace AppProject.Client;

public abstract class BasePage : ComponentBase
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    protected CancellationToken CancellationToken { get; }

    [Inject]
    public IService Service { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public bool IsLoading { get; set; }
    public bool IsCreate { get; set; }

    protected BasePage() => CancellationToken = cancellationTokenSource.Token;
}
