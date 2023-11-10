using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ServerDataProvider.Helpers;

public static class ServiceExtension
{
    public static async Task<T> ReadAsAsync<T>(this HttpResponseMessage response, CancellationToken cancellationToken)
        => await response.Content.ReadAsAsync<T>(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
}
