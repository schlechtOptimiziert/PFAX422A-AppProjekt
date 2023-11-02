using AppProject.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppProject.ServerDataProvider.Interfaces;

public interface IItemPictureService
{
    Task<long> AddItemPictureAsync(ItemPicture picture, long itemId, CancellationToken cancellationToken);
    Task<IEnumerable<ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken);
}
