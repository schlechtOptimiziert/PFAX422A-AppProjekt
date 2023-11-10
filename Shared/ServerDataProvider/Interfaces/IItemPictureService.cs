using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shared.TransferModel;

namespace Shared.ServerDataProvider.Interfaces;

public interface IItemPictureService
{
    Task<long> AddItemPictureAsync(ItemPicture picture, long itemId, CancellationToken cancellationToken);
    Task<IEnumerable<ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken);
    Task DeleteItemPictureAsync(long itemId, long id, CancellationToken cancellationToken);
}
