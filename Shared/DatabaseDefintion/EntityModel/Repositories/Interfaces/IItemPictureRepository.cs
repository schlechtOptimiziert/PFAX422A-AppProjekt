using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TM = TransferModel;

namespace DatabaseDefinition.EntityModel.Repositories.Interfaces;

public interface IItemPictureRepository
{
    Task<long> AddItemPictureAsync(TM.ItemPicture picture, CancellationToken cancellationToken);
    Task<IEnumerable<TM.ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken);
    Task<TM.ItemPicture> GetItemCoverPictureAsync(long itemId, CancellationToken cancellationToken);
    Task DeleteItemPictureAsync(long id, CancellationToken cancellationToken);
}
