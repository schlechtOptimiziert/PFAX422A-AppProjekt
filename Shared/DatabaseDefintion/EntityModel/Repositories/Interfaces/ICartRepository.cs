using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TM = TransferModel;

namespace DatabaseDefinition.EntityModel.Repositories.Interfaces;

public interface ICartRepository
{
    Task AddItemToCartAsync(string userId, long itemId, CancellationToken cancellationToken);
    Task<IEnumerable<TM.CartItemLink>> GetCartItemLinksAsync(string userId, CancellationToken cancellationToken);
    Task DeleteItemFromCartAsync(string userId, long itemId, CancellationToken cancellationToken);
    Task UpdateItemAmountAsync(string userId, long itemId, int amount, CancellationToken cancellationToken);
}
