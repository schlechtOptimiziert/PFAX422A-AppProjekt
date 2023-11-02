using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System.Threading;

namespace AppProject.AdminClient.Pages;

public partial class TestFileUpload : BasePage
{
    string imageUri = string.Empty;
    private async Task UploadFiles(IBrowserFile file)
    {
        imageUri = Helper.ItemPictureToUri(await Helper.BrowserFileToItemPictureAsync(file, 0, CancellationToken).ConfigureAwait(false));
    }
}
