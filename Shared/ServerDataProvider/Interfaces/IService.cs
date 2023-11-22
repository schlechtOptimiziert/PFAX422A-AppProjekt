namespace ServerDataProvider.Interfaces;

public interface IService :
    IItemService,
    IItemPictureService,
    ICartService
{
    HttpClient HttpClient { get; }
}
