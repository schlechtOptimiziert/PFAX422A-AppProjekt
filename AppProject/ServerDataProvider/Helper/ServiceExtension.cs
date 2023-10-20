namespace AppProject.Client.Helpers;

internal static class ServiceExtension
{
    public static async Task<T> ReadAsAsync<T>(this HttpResponseMessage response, CancellationToken cancellationToken)
        => await response.Content.ReadAsAsync<T>(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
}
