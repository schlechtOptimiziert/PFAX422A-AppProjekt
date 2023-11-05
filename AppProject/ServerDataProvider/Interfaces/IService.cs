using System.Net.Http;

namespace AppProject.ServerDataProvider.Interfaces;

public interface IService :
    IItemService,
    IItemPictureService
{
    HttpClient HttpClient { get; }
}
