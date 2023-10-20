namespace AppProject.Client.Interfaces;

public interface IService :
    IItemService
{
    HttpClient HttpClient { get; }
}
