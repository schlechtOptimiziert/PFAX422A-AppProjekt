using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace TransferModel;

public class Item
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
   public ItemPicture CoverPic { get; set; }
   public IEnumerable<ItemPicture> Pictures { get; set; } = default!;
   public string ItemPriceInEuro { get; set; }


   public string GetItemPriceOutEuro() 
   {
      return Price + "€";
   }


}
