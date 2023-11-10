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
    public byte[] Bytes { get; set; }
    public string Description { get; set; }
    public string FileExtension { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Size { get; set; }

    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Item Item { get; set; }
}
