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
        await dbContext.Orders.AddAsync(dbOrder, cancellationToken).ConfigureAwait(false);

        //Add OrderPositions
        foreach (var item in order.Items)
        {
            var orderPosition = new OrderPosition(item, dbOrder.Id);
            await dbContext.OrderPositions.AddAsync(orderPosition, cancellationToken).ConfigureAwait(false);
        }

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return dbOrder.Id;
    }

    public async Task<TM.Order> GetOrderAsync(long orderId, CancellationToken cancellationToken)
    {
        var dbOrder = await dbContext.Orders.Include(o => o.Positions)
                                .SingleOrDefaultAsync(x => x.Id == orderId, cancellationToken)
                                .ConfigureAwait(false) ??
                                    throw new ArgumentException($"Order with id '{orderId}' does not exist.");
        return OrderHelper.MapOrder(dbOrder, dbContext);
    }

    public async Task<IEnumerable<TM.Order>> GetOrdersAsync(string userId, bool includePositions, CancellationToken cancellationToken)
    {
        _ = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken)
                                .ConfigureAwait(false) ??
                                    throw new ArgumentException($"User with id '{userId}' does not exist.");
        var orders = includePositions ? dbContext.Orders.Include(o => o.Positions).AsQueryable() : dbContext.Orders;
        return await orders.Select(o => OrderHelper.MapOrder(o, dbContext))
            .Where(x => x.UserId == userId)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}

public static class OrderHelper
{
    public static Order MapToDbModel(TM.Order from)
    => MapToDbModel(from, null);

    public static Order MapToDbModel(TM.Order from, Order to)
    {
        to ??= new();

        to.Id = from.Id;
        to.UserId = from.UserId;
        to.Date = from.Date;

        return to;
    }

    public static TM.Order MapOrder(Order order, AppProjectDbContext dbContext)
    {
        var tmOrder = new TM.Order
        {
            Id = order.Id,
            UserId = order.UserId,
            Date = order.Date,
            Items = new List<TM.Item>()
        };
        foreach (var position in order.Positions)
        {
            var item = ItemHelper.MapOrderPositionToItem(dbContext, position);
            tmOrder.Items = tmOrder.Items.Append(item);
        }
        return tmOrder;
    }
}