using TransferModel;

namespace ServerDataProvider.Interfaces;

public interface IItemService
{
    Task<long> AddItemAsync(Item item, CancellationToken cancellationToken);
    Task<IEnumerable<Item>> GetItemsAsync(CancellationToken cancellationToken);
    Task<Item> GetItemAsync(long itemId, CancellationToken cancellationToken);
    Task UpdateItemAsync(Item item, CancellationToken cancellationToken);
    Task AddPlatformToItemAsync(long itemId, long platfromId, CancellationToken cancellationToken);
    Task DeleteItemAsync(long id, CancellationToken cancellationToken);
}
