using AppProject.Server.EntityModel;
using Microsoft.EntityFrameworkCore;

namespace AppProject.Server;

internal class DatabaseSetup
{
    public async Task InitializeDatabaseAsync(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppProjectDbContext>();
        await dbContext.Database.MigrateAsync(default).ConfigureAwait(false);
    }
}