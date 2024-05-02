using TransferModel;

namespace ServerDataProvider.Interfaces;

public interface IItemPictureService
{
    Task<IEnumerable<long>> AddItemPicturesAsync(MultipartFormDataContent content, long itemId, CancellationToken cancellationToken);
    Task<IEnumerable<ItemPicture>> GetItemPicturesAsync(long itemId, CancellationToken cancellationToken);
    Task<ItemPicture> GetItemCoverPictureAsync(long itemId, CancellationToken cancellationToken);
    Task DeleteItemPictureAsync(long itemId, long id, CancellationToken cancellationToken);
    Task UpdateAltTextsAsync(long itemId, IEnumerable<ItemPicture> pictures, CancellationToken cancellationToken);
}
