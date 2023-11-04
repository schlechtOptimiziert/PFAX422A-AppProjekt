using Microsoft.AspNetCore.Components.Forms;

namespace AppProject.Server.EntityModel.Repositories.Interfaces;

public interface IItemPictureRepository
{
    Task<long> AddItemPictureAsync(Shared.ItemPicture picture, CancellationToken cancellationToken);
    Task<IEnumerable<Shared.ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken);
    Task DeleteItemPictureAsync(long id, CancellationToken cancellationToken);
}
