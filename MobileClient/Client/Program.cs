using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MobileClient.Client;
using MudBlazor.Services;
using ServerDataProvider.Interfaces;
using ServerDataProvider;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;

namespace MobileClient.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddMudServices();

        builder.Services.AddHttpClient<IService, Service>(
            "MobileClient.ServerAPI",
            client =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        // Supply HttpClient instances that include access tokens when making requests to the server project
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("MobileClient.ServerAPI"));

        builder.Services.AddApiAuthorization();

        await builder.Build().RunAsync();
    }
}