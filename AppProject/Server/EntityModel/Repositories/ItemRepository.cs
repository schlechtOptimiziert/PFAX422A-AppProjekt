using System.Linq.Expressions;
using AppProject.Server.EntityModel;
using AppProject.Server.EntityModel.Database;
using Microsoft.EntityFrameworkCore;
using Server.EntityModel.Repositories.Interfaces;
using Shared = AppProject.Shared;

namespace ÂppProject.Server.EntityModel.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly AppProjectDbContext dbContext;

    public ItemRepository(AppProjectDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<long> AddItemAsync(Shared.Item item, CancellationToken cancellationToken)
    {
        var dbItem = ItemMappings.MapToDbModel(item);
        await dbContext.Items.AddAsync(dbItem, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return dbItem.Id;
    }

    public async Task<IEnumerable<Shared.Item>> GetItemsAsync(CancellationToken cancellationToken)
        => await dbContext.Items.Select(ItemMappings.MapItem)
                                .ToArrayAsync(cancellationToken)
                                .ConfigureAwait(false);

    public async Task<Shared.Item> GetItemAsync(long itemId, CancellationToken cancellationToken)
        => await dbContext.Items.Select(ItemMappings.MapItem)
                                .SingleOrDefaultAsync(x => x.Id == itemId, cancellationToken)
                                .ConfigureAwait(false);

    public async Task UpdateItemAsync(Shared.Item item, CancellationToken cancellationToken)
    {
        var dbItem = await dbContext.Items.SingleAsync(x => x.Id == item.Id, cancellationToken).ConfigureAwait(false) ??
            throw new BadHttpRequestException($"Item with id '{item.Id}' does not exist.");
        ItemMappings.MapToDbModel(item, dbItem);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteItemAsync(long id, CancellationToken cancellationToken)
    {
        var dbItem = await dbContext.Items.SingleAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false) ??
            throw new BadHttpRequestException($"Item with Id '{id}' does not exist.");
        dbContext.Items.Remove(dbItem);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}

public static class ItemMappings
{
    public static Item MapToDbModel(Shared.Item from)
        => MapToDbModel(from, null);

    public static Item MapToDbModel(Shared.Item from, Item to)
    {
        to ??= new();

        to.Name = from.Name;

        return to;
    }

    public static readonly Expression<Func<Item, Shared.Item>> MapItem = (item)
        => Map(item);

    public static IEnumerable<Shared.Item> Map(IEnumerable<Item> from)
        => from.Select(Map);

    public static Shared.Item Map(Item from)
        => Map(from, null);

    public static Shared.Item Map(Item from, Shared.Item to)
    {
        to ??= new();

        to.Id = from.Id;
        to.Name = from.Name;

        return to;
    }
}
