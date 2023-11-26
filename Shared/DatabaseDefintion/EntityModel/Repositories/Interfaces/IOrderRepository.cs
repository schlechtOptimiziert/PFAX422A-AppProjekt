using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TM = TransferModel;

namespace DatabaseDefinition.EntityModel.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<long> AddOrderAsync(TM.Order order, CancellationToken cancellationToken);
    Task<IEnumerable<TM.Order>> GetOrdersAsync(string userId, bool includePositions, CancellationToken cancellationToken);
    Task<TM.Order> GetOrderAsync(long orderId, CancellationToken cancellationToken);
}
