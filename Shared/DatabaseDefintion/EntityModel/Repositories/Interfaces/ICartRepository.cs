using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TM = TransferModel;

namespace DatabaseDefinition.EntityModel.Repositories.Interfaces;

public interface ICartRepository
{
    Task<IEnumerable<TM.CartItemLink>> GetCartItemLinksAsync(string userId, CancellationToken cancellationToken);
    Task AddItemToCart(string userId, long itemId, CancellationToken cancellationToken);
    Task DeleteItemFromCart(string userId, long itemId, CancellationToken cancellationToken);
    Task UpdateItemAmount(string userId, long itemId, int amount, CancellationToken cancellationToken);
}
