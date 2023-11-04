using AppProject.Server.EntityModel.Database;
using AppProject.Server.EntityModel.Repositories.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace AppProject.Server.EntityModel.Repositories;

public class ItemPictureRepository : IItemPictureRepository
{
    private readonly AppProjectDbContext dbContext;

    public ItemPictureRepository(AppProjectDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<long> AddItemPictureAsync(Shared.ItemPicture picture, CancellationToken cancellationToken)
    {
        var dbItemPicture = ItemPictureMappings.MapToDbModel(picture);
        await dbContext.ItemPictures.AddAsync(dbItemPicture, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return dbItemPicture.Id;
    }

    public async Task<IEnumerable<Shared.ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken)
        => await dbContext.ItemPictures.Select(ItemPictureMappings.MapItemPicture)
            .Where(x => x.ItemId == itemId)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task DeleteItemPictureAsync(long id, CancellationToken cancellationToken)
    {
        var dbItemPicture = await dbContext.ItemPictures.SingleAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false) ??
            throw new BadHttpRequestException($"Item picture with Id '{id}' does not exist.");
        dbContext.ItemPictures.Remove(dbItemPicture);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}

public static class ItemPictureMappings
{
    public static ItemPicture MapToDbModel(Shared.ItemPicture from)
        => MapToDbModel(from, null);

    public static ItemPicture MapToDbModel(Shared.ItemPicture from, ItemPicture to)
    {
        to ??= new();

        to.ItemId = from.ItemId;
        to.Bytes = from.Bytes;
        to.Description = from.Description;
        to.FileExtension = from.FileExtension;
        to.Size = from.Size;

        return to;
    }

    public static readonly Expression<Func<ItemPicture, Shared.ItemPicture>> MapItemPicture = (picture)
        => new Shared.ItemPicture
        {
            Id = picture.Id,
            ItemId = picture.ItemId,
            Bytes = picture.Bytes,
            Description = picture.Description,
            FileExtension= picture.FileExtension,
            Size = picture.Size,
        };
}
