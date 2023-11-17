using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransferModel;

namespace ServerDataProvider.Interfaces;

public interface IItemPictureService
{
    Task<long> AddItemPictureAsync(ItemPicture picture, long itemId, CancellationToken cancellationToken);
    Task<IEnumerable<ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken);
    Task DeleteItemPictureAsync(long itemId, long id, CancellationToken cancellationToken);
    Task<ItemPicture> GetItemCoverPic(long itemId, CancellationToken cancellationToken);
}
