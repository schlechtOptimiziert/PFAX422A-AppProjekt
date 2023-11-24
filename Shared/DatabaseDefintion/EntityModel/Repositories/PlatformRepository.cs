using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DatabaseDefinition.EntityModel;
using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using DatabaseDefintion.EntityModel.Database;
using Microsoft.EntityFrameworkCore;
using TM = TransferModel;

namespace DatabaseDefintion.EntityModel.Repositories;

public class PlatformRepository : IPlatformRepositroy
{
    private readonly AppProjectDbContext dbContext;

    public PlatformRepository(AppProjectDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<long> AddPlatformAsync(TM.Platform platform, CancellationToken cancellationToken)
    {
        var dbPlatform = PlatformHelper.MapToDbModel(platform);
        await dbContext.Platforms.AddAsync(dbPlatform, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return dbPlatform.Id;
    }

    public async Task<IEnumerable<TM.Platform>> GetPlatformsAsync(CancellationToken cancellationToken)
        => await dbContext.Platforms.Select(PlatformHelper.MapPlatform)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<TM.Platform> GetPlatformAsync(long platformId, CancellationToken cancellationToken)
    {
        await PlatformHelper.ThrowIfItemDoesNotExistAsync(dbContext, platformId, cancellationToken).ConfigureAwait(false);

        return await dbContext.Platforms.Select(PlatformHelper.MapPlatform)
                .SingleOrDefaultAsync(x => x.Id == platformId, cancellationToken)
                .ConfigureAwait(false);
    }

    public async Task UpdatePlatformAsync(TM.Platform platform, CancellationToken cancellationToken)
    {
        var dbPlatform = await dbContext.Platforms.FirstOrDefaultAsync(x => x.Id == platform.Id, cancellationToken).ConfigureAwait(false) ??
            throw new ArgumentException($"Platform with id '{platform.Id}' does not exist.");
        PlatformHelper.MapToDbModel(platform, dbPlatform);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeletePlatformAsync(long platformId, CancellationToken cancellationToken)
    {
        var dbPlatform = await dbContext.Platforms.FirstOrDefaultAsync(x => x.Id == platformId, cancellationToken).ConfigureAwait(false) ??
            throw new ArgumentException($"Platform with id '{platformId}' does not exist.");
        dbContext.Platforms.Remove(dbPlatform);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}

public static class PlatformHelper
{
    public static Platform MapToDbModel(TM.Platform from)
        => MapToDbModel(from, null);

    public static Platform MapToDbModel(TM.Platform from, Platform to)
    {
        to ??= new();

        to.Name = from.Name;

        return to;
    }

    public static readonly Expression<Func<Platform, TM.Platform>> MapPlatform = (platform)
        => new TM.Platform
        {
            Id = platform.Id,
            Name = platform.Name,
        };

    public static async Task ThrowIfItemDoesNotExistAsync(AppProjectDbContext dbContext, long platformId, CancellationToken cancellationToken)
        => _ = await dbContext.Platforms.Select(MapPlatform)
                    .SingleOrDefaultAsync(x => x.Id == platformId, cancellationToken)
                    .ConfigureAwait(false) ??
                        throw new ArgumentException($"Platform with id '{platformId}' does not exist.");
}