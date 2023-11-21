using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using DatabaseDefintion.EntityModel.Database;
using Microsoft.EntityFrameworkCore;
using TM = TransferModel;

namespace DatabaseDefinition.EntityModel.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppProjectDbContext dbContext;

    public CartRepository(AppProjectDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task AddItemToCartAsync(string userId, long itemId, CancellationToken cancellationToken)
    {
        await ItemHelper.ThrowIfItemDoesNotExistAsync(dbContext, itemId, cancellationToken).ConfigureAwait(false);
        _ = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken)
                                .ConfigureAwait(false) ??
                                    throw new ArgumentException($"User with id '{userId}' does not exist.");
        var cartItemLink = await dbContext.CartItemLinks.SingleOrDefaultAsync(x => x.ItemId == itemId && x.UserId == userId, cancellationToken)
                                                        .ConfigureAwait(false);
        if (cartItemLink != null)
            cartItemLink.Amount++;
        else
            await dbContext.CartItemLinks.AddAsync(new CartItemLink() { UserId = userId, ItemId = itemId, Amount = 1 });
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TM.CartItemLink>> GetCartItemLinksAsync(string userId, CancellationToken cancellationToken)
    {
        _ = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken)
                                .ConfigureAwait(false) ??
                                    throw new ArgumentException($"User with id '{userId}' does not exist.");

        var cartItemLinks = await dbContext.CartItemLinks.Include(x => x.Item)
            .Select(CartItemLinkHelper.MapCartItemLink)
            .Where(x => x.UserId == userId)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);
        return cartItemLinks;
    }

    public async Task DeleteItemFromCartAsync(string userId, long itemId, CancellationToken cancellationToken)
    {
        await ItemHelper.ThrowIfItemDoesNotExistAsync(dbContext, itemId, cancellationToken).ConfigureAwait(false);
        _ = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken)
                                .ConfigureAwait(false) ??
                                    throw new ArgumentException($"User with id '{userId}' does not exist.");
        var cartItemLink = await dbContext.CartItemLinks.FirstOrDefaultAsync(x => x.ItemId == itemId, cancellationToken)
                                                        .ConfigureAwait(false) ??
                                                            throw new ArgumentException($"CartItemLink with itemId '{itemId}' does not exist.");
        dbContext.CartItemLinks.Remove(cartItemLink);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateItemAmountAsync(string userId, long itemId, int amount, CancellationToken cancellationToken)
    {
        await ItemHelper.ThrowIfItemDoesNotExistAsync(dbContext, itemId, cancellationToken).ConfigureAwait(false);
        _ = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken)
                                .ConfigureAwait(false) ??
                                    throw new ArgumentException($"User with id '{userId}' does not exist.");
        var cartItemLink = await dbContext.CartItemLinks.FirstOrDefaultAsync(x => x.ItemId == itemId, cancellationToken)
                                                        .ConfigureAwait(false) ??
                                                            throw new ArgumentException($"CartItemLink with itemId '{itemId}' does not exist.");
        cartItemLink.Amount = amount;
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}

public static class CartItemLinkHelper
{
    public static CartItemLink MapToDbModel(TM.CartItemLink from)
        => MapToDbModel(from, null);

    public static CartItemLink MapToDbModel(TM.CartItemLink from, CartItemLink to)
    {
        to ??= new();

        to.UserId = from.UserId;
        to.ItemId = from.ItemId;
        to.Amount = from.Amount;

        return to;
    }

    public static readonly Expression<Func<CartItemLink, TM.CartItemLink>> MapCartItemLink = (cartItemLink)
        => new TM.CartItemLink
        {
            UserId = cartItemLink.UserId,
            ItemId = cartItemLink.ItemId,
            Amount = cartItemLink.Amount,
            Item = ItemHelper.MapItemToTM(cartItemLink.Item)
        };
}
