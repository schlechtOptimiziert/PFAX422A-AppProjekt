var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapWhen(ctx => ctx.Request.Host.Port == 5001 ||
    ctx.Request.Host.Equals("firstapp.com"), first =>
    {
        first.Use((ctx, nxt) =>
        {
            ctx.Request.Path = "/FirstApp" + ctx.Request.Path;
            return nxt();
        });

        first.UseBlazorFrameworkFiles("/FirstApp");
        first.UseStaticFiles();
        first.UseStaticFiles("/FirstApp");
        first.UseRouting();

        first.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("/FirstApp/{*path:nonfile}",
                "FirstApp/index.html");
        });
    });

app.MapWhen(ctx => ctx.Request.Host.Port == 5002 ||
    ctx.Request.Host.Equals("secondapp.com"), second =>
    {
        second.Use((ctx, nxt) =>
        {
            ctx.Request.Path = "/SecondApp" + ctx.Request.Path;
            return nxt();
        });

        second.UseBlazorFrameworkFiles("/SecondApp");
        second.UseStaticFiles();
        second.UseStaticFiles("/SecondApp");
        second.UseRouting();

        second.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("/SecondApp/{*path:nonfile}",
                "SecondApp/index.html");
        });
    });

app.UseStaticFiles();

app.Run();
