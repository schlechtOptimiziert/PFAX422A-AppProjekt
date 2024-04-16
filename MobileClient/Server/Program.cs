using System;
using System.Configuration;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using TM = TransferModel;

namespace MobileClient;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "AppProject");
        builder.Configuration.AddJsonFile(Path.Combine(baseFolder, "appsettings.json"), true);
        builder.Configuration.AddEnvironmentVariables();

        var serviceConfig = builder.Configuration.Get<TM.ServiceConfiguration>();
        if (serviceConfig?.ConnectionStrings is null)
        {
            var DbConString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                ?? throw new ConfigurationErrorsException("DB Connectionstring can not be set. Appsettings.json was not found and no Environment Variable was set.");

            serviceConfig = new TM.ServiceConfiguration()
            {
                ConnectionStrings = new()
                {
                    Db = DbConString,
                },
            };
        }

        builder.Services.AddSingleton(serviceConfig);

        builder.Services.AddDatabase(serviceConfig.ConnectionStrings.Db);

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "uploads")),
            RequestPath = "/Images"
        });

        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}