using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TM = TransferModel;

namespace DatabaseDefinition.EntityModel.Repositories.Interfaces;

public interface IItemRepository
{
    Task<long> AddItemAsync(TM.Item item, CancellationToken cancellationToken);
    Task<IEnumerable<TM.Item>> GetItemsAsync(CancellationToken cancellationToken);
    Task<TM.Item> GetItemAsync(long itemId, CancellationToken cancellationToken);
    Task UpdateItemAsync(TM.Item item, CancellationToken cancellationToken);
    Task DeleteItemAsync(long id, CancellationToken cancellationToken);
}
