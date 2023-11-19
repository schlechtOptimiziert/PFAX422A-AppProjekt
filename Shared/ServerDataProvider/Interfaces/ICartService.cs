using TransferModel;

namespace ServerDataProvider.Interfaces;

public interface ICartService
{
    public Task AddItemToCartAsync(string userId, long itemId, CancellationToken cancellationToken);
    public Task<IEnumerable<CartItemLink>> GetCartItemLinksAsync(string userId, CancellationToken cancellationToken);
    public Task UpdateItemCartAmountAsync(string userId, CartItemLink cartItemLink, CancellationToken cancellationToken);
    public Task DeleteItemAsync(string userId, long itemId, CancellationToken cancellationToken);
}
