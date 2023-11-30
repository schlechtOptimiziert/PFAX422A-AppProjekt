using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DatabaseDefinition.EntityModel.Database;
using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using TM = TransferModel;

namespace DatabaseDefinition.EntityModel.Repositories;

public class ItemPictureRepository : IItemPictureRepository
{
    private readonly AppProjectDbContext dbContext;

    public ItemPictureRepository(AppProjectDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<long> AddItemPictureAsync(TM.ItemPicture picture, CancellationToken cancellationToken)
    {
        var dbItemPicture = ItemPictureHelper.MapToDbModel(picture);
        await dbContext.ItemPictures.AddAsync(dbItemPicture, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return dbItemPicture.Id;
    }

    public async Task<IEnumerable<TM.ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken)
    {
        await ItemHelper.ThrowIfItemDoesNotExistAsync(dbContext, itemId, cancellationToken).ConfigureAwait(false);
        return await dbContext.ItemPictures.Select(ItemPictureHelper.MapItemPicture)
            .Where(x => x.ItemId == itemId)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<TM.ItemPicture> GetItemCoverPictureAsync(long itemId, CancellationToken cancellationToken)
    {
        return await dbContext.ItemPictures.Select(ItemPictureHelper.MapItemPicture)
                .FirstOrDefaultAsync(x => x.ItemId == itemId, cancellationToken)
                .ConfigureAwait(false);
    }

    public async Task DeleteItemPictureAsync(long id, CancellationToken cancellationToken)
    {
        var dbItemPicture = await dbContext.ItemPictures.FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false) ??
            throw new ArgumentException($"Item picture with Id '{id}' does not exist.");
        dbContext.ItemPictures.Remove(dbItemPicture);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}

public static class ItemPictureHelper
{
    public static ItemPicture MapToDbModel(TM.ItemPicture from)
        => MapToDbModel(from, null);

    public static ItemPicture MapToDbModel(TM.ItemPicture from, ItemPicture to)
    {
        to ??= new();

        to.ItemId = from.ItemId;
        to.FileName = from.FileName;

        return to;
    }

    public static readonly Expression<Func<ItemPicture, TM.ItemPicture>> MapItemPicture = (picture)
        => new TM.ItemPicture
        {
            Id = picture.Id,
            ItemId = picture.ItemId,
            FileName = picture.FileName,
        };
}
