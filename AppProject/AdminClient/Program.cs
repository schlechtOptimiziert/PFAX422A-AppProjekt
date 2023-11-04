using AppProject.AdminClient;
using AppProject.ServerDataProvider;
using AppProject.ServerDataProvider.Interfaces;
using AppProject.SharedClientComponents.Toast;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddBlazoredToast();

builder.Services.AddScoped<ToastService>();

builder.Services.AddHttpClient<IService, Service>(
    "AdminClient",
    client =>
    {
        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    });

await builder.Build().RunAsync();
