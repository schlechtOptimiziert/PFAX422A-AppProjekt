using AdminClient.Client;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using ServerDataProvider.Interfaces;
using ServerDataProvider;
using Blazored.Toast.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

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

        builder.Services.AddHttpClient(
            "AdminClient.ServerAPI",
            client =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        // Supply HttpClient instances that include access tokens when making requests to the server project
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AdminClient.ServerAPI"));

        builder.Services.AddApiAuthorization();

        await builder.Build().RunAsync();
    }
}
