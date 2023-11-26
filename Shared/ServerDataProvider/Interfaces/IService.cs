namespace ServerDataProvider.Interfaces;

public interface IService :
    IItemService,
    IItemPictureService,
    ICartService,
    IPlatformService,
    IOrderService
{
    HttpClient HttpClient { get; }
}
