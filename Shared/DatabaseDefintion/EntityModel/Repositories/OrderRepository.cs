using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DatabaseDefinition.EntityModel;
using DatabaseDefinition.EntityModel.Database;
using DatabaseDefinition.EntityModel.Repositories;
using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using TM = TransferModel;

namespace DatabaseDefintion.EntityModel.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppProjectDbContext dbContext;

    public OrderRepository(AppProjectDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<long> AddOrderAsync(TM.Order order, CancellationToken cancellationToken)
    {
        var dbOrder = OrderHelper.MapToDbModel(order);

        //Add BillingAddress
        var billingAddress = new BillingAddress
        {
            Name = order.Name,
            Street = order.Street,
            StreetNumber = order.StreetNumber,
            Postcode = order.Postcode,
            City = order.City,
            Country = order.Country
        };
        await dbContext.BillingAddresses.AddAsync(billingAddress, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false); //we need to save multiple times, because else there we dont know the ids of billingAddress and order
        dbOrder.BillingAddressId = billingAddress.Id;
        await dbContext.Orders.AddAsync(dbOrder, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        //Add OrderPositions and remove items from cart
        var cartItemLinks = dbContext.CartItemLinks.Include(c => c.Item).Where(c => c.UserId == order.UserId);
        foreach (var cartItemLink in cartItemLinks)
        {
            var orderPosition = new OrderPosition(cartItemLink.Item, dbOrder.Id, cartItemLink.Amount);
            await dbContext.OrderPositions.AddAsync(orderPosition, cancellationToken).ConfigureAwait(false);
            dbContext.CartItemLinks.Remove(cartItemLink);
        }

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return dbOrder.Id;
    }

    public async Task<TM.Order> GetOrderAsync(long orderId, CancellationToken cancellationToken)
    {
        var dbOrder = await dbContext.Orders.Include(o => o.Positions)
                                .Include(o => o.BillingAddress)
                                .SingleOrDefaultAsync(x => x.Id == orderId, cancellationToken)
                                .ConfigureAwait(false) ??
                                    throw new ArgumentException($"Order with id '{orderId}' does not exist.");
        return OrderHelper.MapOrder(dbOrder, true, dbContext);
    }

    public async Task<IEnumerable<TM.Order>> GetOrdersAsync(string userId, bool includePositions, CancellationToken cancellationToken)
    {
        _ = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken)
                                .ConfigureAwait(false) ??
                                    throw new ArgumentException($"User with id '{userId}' does not exist.");
        var orders = dbContext.Orders.Include(o => o.BillingAddress).AsQueryable();
        orders = includePositions ? orders.Include(o => o.Positions).AsQueryable() : orders;
        var userOrders = await orders.Where(x => x.UserId == userId)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);
        List<TM.Order> mappedOrders = new();
        foreach (var order in userOrders)
            mappedOrders.Add(OrderHelper.MapOrder(order, includePositions, dbContext));
        return mappedOrders;
    }
}

public static class OrderHelper
{
    public static Order MapToDbModel(TM.Order from)
    => MapToDbModel(from, null);

    public static Order MapToDbModel(TM.Order from, Order to)
    {
        to ??= new();

        to.UserId = from.UserId;
        to.Date = from.Date;

        return to;
    }

    public static TM.Order MapOrder(Order order, bool includePositions, AppProjectDbContext dbContext)
    {
        var tmOrder = new TM.Order
        {
            Id = order.Id,
            UserId = order.UserId,
            Date = order.Date,
            Name = order.BillingAddress.Name,
            Street = order.BillingAddress.Street,
            StreetNumber = order.BillingAddress.StreetNumber,
            Postcode = order.BillingAddress.Postcode,
            City = order.BillingAddress.City,
            Country = order.BillingAddress.Country,
        };
        if (!includePositions)
            return tmOrder;
        tmOrder.Items = new List<TM.Item>();
        foreach (var position in order.Positions)
        {
            for (int i = 0; i < position.Amount; i++)
            {
                var item = ItemHelper.MapOrderPositionToItem(dbContext, position);
                tmOrder.Items = tmOrder.Items.Append(item);
            }
        }
        return tmOrder;
    }
}