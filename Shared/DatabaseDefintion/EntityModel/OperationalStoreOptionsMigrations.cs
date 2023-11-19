﻿using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;

namespace DatabaseDefintion.EntityModel;

public class OperationalStoreOptionsMigrations : IOptions<OperationalStoreOptions>
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