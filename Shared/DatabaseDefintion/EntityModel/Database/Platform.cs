using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseDefintion.EntityModel.Database;

[PrimaryKey("Id")]
public class Platform
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Name { get; set; }

    [ForeignKey("PlatformId")]
    public IEnumerable<ItemPlatformLink> ItemPlatformLinks { get; set; }
}
