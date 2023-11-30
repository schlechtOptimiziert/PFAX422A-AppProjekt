using System.Collections.Generic;
using System.Threading.Tasks;
using ClientComponents.Components;
using Microsoft.AspNetCore.Components;
using TransferModel;

namespace MobileClient.Client.Components;

public partial class HorizontalProductScroller : BaseComponent
{
    [Parameter]
    public string Class { get; set; } = default!;
    [Parameter]
    public string Style { get; set; } = default!;
    [Parameter]
    public string Title { get; set; } = default!;

    [Parameter]
    public IEnumerable<Item> Items { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync().ConfigureAwait(false);

        foreach (var item in Items)
        {
            var coverPicture = await Service.GetItemCoverPictureAsync(item.Id, CancellationToken).ConfigureAwait(false);

            if (coverPicture != null)
                item.CoverPicturePath = $"/Images/{coverPicture.FileName}";
        }
    }

    private void NavigateToList(long itemId)
        => NavigationManager.NavigateTo($"/items/{itemId}");
}

