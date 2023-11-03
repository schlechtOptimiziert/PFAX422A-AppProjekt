﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppProject.Server.EntityModel.Database;

[PrimaryKey("Id")]
public class Item
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [Column(TypeName = "decimal(16, 2)")]
    public decimal Price { get; set; }
}
