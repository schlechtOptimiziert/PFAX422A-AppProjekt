using System.ComponentModel.DataAnnotations.Schema;
using DatabaseDefinition.EntityModel.Database;
using Microsoft.EntityFrameworkCore;

namespace DatabaseDefintion.EntityModel.Database;

[PrimaryKey("UserId", "ItemId")]
public class CartItemLink
{
    [ForeignKey("User")]
    public string UserId { get; set; }
    [ForeignKey("Item")]
    public long ItemId { get; set; }
    public int Amount { get; set; }

    //[DeleteBehavior(DeleteBehavior.NoAction)]
    public ApplicationUser User { get; set; }
    //[DeleteBehavior(DeleteBehavior.NoAction)]
    public Item Item { get; set; }
}
