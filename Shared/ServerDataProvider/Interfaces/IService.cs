using System.Net.Http;

namespace Shared.ServerDataProvider.Interfaces;

public interface IService :
    IItemService,
    IItemPictureService
{
    HttpClient HttpClient { get; }
}
