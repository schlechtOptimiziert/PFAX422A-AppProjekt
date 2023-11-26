using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferModel;

namespace AdminClient.Client.Pages;

partial class Platforms : BasePage
{
    private IEnumerable<Platform> platforms = Enumerable.Empty<Platform>();

    protected override async Task OnInitializedAsync()
    {
        platforms = await LoadPlatformsAsync().ConfigureAwait(false);
    }

    private async Task<IEnumerable<Platform>> LoadPlatformsAsync()
        => await Service.GetPlatformsAsync(CancellationToken).ConfigureAwait(false) ?? Enumerable.Empty<Platform>();

    private void GoToDetails(long id)
        => NavigationManager.NavigateTo($"/platforms/{id}");
}