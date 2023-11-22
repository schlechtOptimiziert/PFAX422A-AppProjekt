namespace TransferModel;

public class CartItemLink
{
    public string UserId { get; set; }
    public long ItemId { get; set; }
    public int Amount { get; set; }

    public Item Item { get; set; }
}
