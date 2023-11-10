﻿using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json;
using Shared.DatabaseDefinition.EntityModel;
using TM = Shared.TransferModel;

namespace DatabaseDefintion.EntityModel;

public class AppProjectDbContextFactory : IDesignTimeDbContextFactory<AppProjectDbContext>
{
    public AppProjectDbContext CreateDbContext(string[] args)
    {
        var config = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "AppProject", "appsettings.json");
        var serviceConfiguration = JsonConvert.DeserializeObject<TM.ServiceConfiguration>(File.ReadAllText(config));

        var builder = new DbContextOptionsBuilder<AppProjectDbContext>();
        builder.UseSqlServer(serviceConfiguration.ConnectionStrings.Db);
        return new AppProjectDbContext(builder.Options);
    }
}