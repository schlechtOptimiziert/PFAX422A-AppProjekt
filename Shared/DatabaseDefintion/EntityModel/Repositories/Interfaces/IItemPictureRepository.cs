using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TM = Shared.TransferModel;

namespace Shared.DatabaseDefinition.EntityModel.Repositories.Interfaces;

public interface IItemPictureRepository
{
    Task<long> AddItemPictureAsync(TM.ItemPicture picture, CancellationToken cancellationToken);
    Task<IEnumerable<TM.ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken);
    Task DeleteItemPictureAsync(long id, CancellationToken cancellationToken);
}
