using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseDefinition.EntityModel.Database;

[PrimaryKey("Id")]
public class ItemPicture
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [ForeignKey("Item")]
    public long ItemId { get; set; }
    public string FileName { get; set; }

    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Item Item { get; set; }
}
