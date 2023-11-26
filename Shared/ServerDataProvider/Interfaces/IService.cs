namespace ServerDataProvider.Interfaces;

public interface IService :
    IItemService,
    IItemPictureService,
    ICartService,
    IOrderService
{
    HttpClient HttpClient { get; }
}
