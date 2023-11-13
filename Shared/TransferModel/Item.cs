using System.Collections.Generic;

namespace TransferModel;

public class Item
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
   public ItemPicture CoverPic { get; set; }
   public IEnumerable<ItemPicture> Pictures { get; set; } = default!;

   // string ItemPriceInEuro
   // getITemPriceInEuro()
}
