using AppProject.Client.Interfaces;

namespace AppProject.Client;

public partial class Service : IService
{
    private readonly HttpClient httpClient;

    public Service(HttpClient httpClient)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public HttpClient HttpClient => httpClient;
}
