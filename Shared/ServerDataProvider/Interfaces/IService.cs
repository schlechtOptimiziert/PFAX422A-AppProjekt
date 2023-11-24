namespace ServerDataProvider.Interfaces;

public interface IService :
    IItemService,
    IItemPictureService,
    ICartService,
    IPlatformService
{
    HttpClient HttpClient { get; }
}
