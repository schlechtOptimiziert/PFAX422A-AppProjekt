using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MobileClient.Client;
using MudBlazor.Services;
using ServerDataProvider;
using ServerDataProvider.Interfaces;
using System;
using System.Threading.Tasks;

namespace MobileClient.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddMudServices();

            builder.Services.AddHttpClient<IService, Service>(
                "MobileClient",
                client =>
                {
                    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                });

            await builder.Build().RunAsync();
        }
    }
}