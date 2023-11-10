using System.Net.Http;

namespace ServerDataProvider.Interfaces;

public interface IService :
    IItemService,
    IItemPictureService
{
    HttpClient HttpClient { get; }
}
