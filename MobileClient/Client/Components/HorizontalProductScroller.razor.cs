using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using TransferModel;
using MobileClient.Client.Pages;
using System.Threading.Tasks;
using ClientComponents.Components;

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

   private string GetItemCoverPictureUri(Item item)
   {
      //await fehlt bzw es gibt fehler
       var itemPicture = Service.GetItemCoverPictureAsync(item.Id, CancellationToken).ConfigureAwait(false);
       ItemPicture result = itemPicture.GetAwaiter().GetResult();
       return ItemPicture.ItemPictureToUri(result);
      
   }
}

