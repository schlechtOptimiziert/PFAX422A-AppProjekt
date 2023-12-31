﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DatabaseDefinition.EntityModel.Database;
using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using DatabaseDefintion.EntityModel.Database;
using DatabaseDefintion.EntityModel.Repositories;
using Microsoft.EntityFrameworkCore;
using TM = TransferModel;

namespace DatabaseDefinition.EntityModel.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly AppProjectDbContext dbContext;

    public ItemRepository(AppProjectDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<long> AddItemAsync(TM.Item item, CancellationToken cancellationToken)
    {
        var dbItem = ItemHelper.MapToDbModel(item);
        await dbContext.Items.AddAsync(dbItem, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return dbItem.Id;
    }

    public async Task AddPlatformToItemAsync(long itemId, long platformId, CancellationToken cancellationToken)
    {
        var dbItemPlatformLink = new ItemPlatformLink()
        {
            ItemId = itemId,
            PlatformId = platformId,
        };
        await dbContext.ItemPlatformLinks.AddAsync(dbItemPlatformLink, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TM.Item>> GetItemsAsync(CancellationToken cancellationToken)
        => await dbContext.Items.Select(ItemHelper.MapItem)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<TM.Item> GetItemAsync(long itemId, CancellationToken cancellationToken)
    {
        var outcome = await dbContext.Items.Select(ItemHelper.MapItem)
                .SingleOrDefaultAsync(x => x.Id == itemId, cancellationToken)
                .ConfigureAwait(false)
                    ?? throw new ArgumentException($"Item with id '{itemId}' does not exist.");
        outcome.Platforms = await dbContext.ItemPlatformLinks.Include(x => x.Platform)
            .Where(x => x.ItemId == itemId)
            .Select(x => x.Platform)
            .Select(PlatformHelper.MapPlatform)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);
        return outcome;
    }

    public async Task<IEnumerable<TM.Item>> GetPlatformFilteredItemsAsync(long platformId, CancellationToken cancellationToken)
    {
        _ = await dbContext.Platforms.Select(PlatformHelper.MapPlatform)
            .SingleOrDefaultAsync(x => x.Id == platformId, cancellationToken)
            .ConfigureAwait(false)
                ?? throw new ArgumentException($"Platform with id '{platformId}' does not exist.");
        return await dbContext.ItemPlatformLinks
                .Include(x => x.Item)
                .Where(x => x.PlatformId == platformId)
                .Select(x => x.Item)
                .Select(ItemHelper.MapItem)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);
    }

    public async Task UpdateItemAsync(TM.Item item, CancellationToken cancellationToken)
    {
        var dbItem = await dbContext.Items.FirstOrDefaultAsync(x => x.Id == item.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new ArgumentException($"Item with id '{item.Id}' does not exist.");
        ItemHelper.MapToDbModel(item, dbItem);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task RemovePlatformFromItemAsync(long itemId, long platformId, CancellationToken cancellationToken)
    {
        var dbItemPlatformLink = await dbContext.ItemPlatformLinks.FirstOrDefaultAsync(x => x.ItemId == itemId && x.PlatformId == platformId, cancellationToken).ConfigureAwait(false)
            ?? throw new ArgumentException($"ItemPlatfromLink with itemId '{itemId}' and platformId '{platformId}' does not exist.");
        dbContext.ItemPlatformLinks.Remove(dbItemPlatformLink);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteItemAsync(long id, CancellationToken cancellationToken)
    {
        var dbItem = await dbContext.Items.FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false)
            ?? throw new ArgumentException($"Item with Id '{id}' does not exist.");
        dbContext.Items.Remove(dbItem);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}

public static class ItemHelper
{
    public static Item MapToDbModel(TM.Item from)
        => MapToDbModel(from, null);

    public static Item MapToDbModel(TM.Item from, Item to)
    {
        to ??= new();

        to.Name = from.Name;
        to.Description = from.Description;
        to.Price = from.Price;

        return to;
    }

    public static readonly Expression<Func<Item, TM.Item>> MapItem = (item)
        => new()
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
        };

    public static TM.Item MapItemToTM(Item item)
        => new()
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
        };

    public static async Task ThrowIfItemDoesNotExistAsync(AppProjectDbContext dbContext, long itemId, CancellationToken cancellationToken)
        => _ = await dbContext.Items.Select(MapItem)
                        .SingleOrDefaultAsync(x => x.Id == itemId, cancellationToken)
                        .ConfigureAwait(false) ??
                            throw new ArgumentException($"Item with id '{itemId}' does not exist.");

    public static TM.Item MapOrderPositionToItem(AppProjectDbContext dbContext, OrderPosition orderPosition)
    {
        var item = dbContext.Items.Select(MapItem)
                        .SingleOrDefault(x => x.Id == orderPosition.ItemId) ??
                            throw new ArgumentException($"Item with id '{orderPosition.ItemId}' does not exist.");
        item.Price = orderPosition.Price;
        item.Name = orderPosition.Name;
        return item;
    }
}
