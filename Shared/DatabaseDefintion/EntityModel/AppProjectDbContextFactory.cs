using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json;
using DatabaseDefinition.EntityModel;
using TM = TransferModel;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;

namespace DatabaseDefintion.EntityModel;

public class AppProjectDbContextFactory : IDesignTimeDbContextFactory<AppProjectDbContext>
{
    public AppProjectDbContext CreateDbContext(string[] args)
    {
        var config = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "AppProject", "appsettings.json");
        var serviceConfiguration = JsonConvert.DeserializeObject<TM.ServiceConfiguration>(File.ReadAllText(config));

        var builder = new DbContextOptionsBuilder<AppProjectDbContext>();
        builder.UseSqlServer(serviceConfiguration.ConnectionStrings.Db);
        return new AppProjectDbContext(builder.Options, new OperationalStoreOptionsMigrations());
    }
}

class OperationalStoreOptionsMigrations : IOptions<OperationalStoreOptions>
{
    public OperationalStoreOptions Value => new OperationalStoreOptions()
    {
        DeviceFlowCodes = new TableConfiguration("DeviceCodes"),
        EnableTokenCleanup = false,
        PersistedGrants = new TableConfiguration("PersistedGrants"),
        TokenCleanupBatchSize = 100,
        TokenCleanupInterval = 3600,
    };
}