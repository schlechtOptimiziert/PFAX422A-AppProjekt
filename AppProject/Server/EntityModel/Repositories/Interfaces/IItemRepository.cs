using Shared = AppProject.Shared;

namespace Server.EntityModel.Repositories.Interfaces;

public interface IItemRepository
{
    Task<long> AddItemAsync(Shared.Item item, CancellationToken cancellationToken);
    Task<IEnumerable<Shared.Item>> GetItemsAsync(CancellationToken cancellationToken);
    Task<Shared.Item> GetItemAsync(long itemId, CancellationToken cancellationToken);
    Task UpdateItemAsync(Shared.Item item, CancellationToken cancellationToken);
    Task DeleteItemAsync(long id, CancellationToken cancellationToken);
}
