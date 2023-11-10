using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using ServerDataProvider.Interfaces;
using ServerDataProvider;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ClientComponents.Toast;
using Blazored.Toast;

namespace AdminClient.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
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
    }
}