using AppProject.Shared;

namespace AppProject.Client.Interfaces;


public interface IItemService
{
    Task<long> AddItemAsync(Item item, CancellationToken cancellationToken);
    Task<IEnumerable<Item>> GetItemsAsync(long campaignId, CancellationToken cancellationToken);
    Task<Item> GetItemAsync(long itemId, CancellationToken cancellationToken);
    Task UpdateItemAsync(Item item, CancellationToken cancellationToken);
    Task DeleteItemAsync(long id, CancellationToken cancellationToken);
}
