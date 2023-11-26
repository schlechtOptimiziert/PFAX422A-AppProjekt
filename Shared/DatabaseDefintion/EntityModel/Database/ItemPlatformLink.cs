using DatabaseDefinition.EntityModel.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseDefintion.EntityModel.Database;

[PrimaryKey("ItemId", "PlatformId")]
public class ItemPlatformLink
{
    [ForeignKey("Item")]
    public long ItemId { get; set; }
    [ForeignKey("Platform")]
    public long PlatformId { get; set; }

    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Item Item { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Platform Platform { get; set; }
}
