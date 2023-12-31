﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DatabaseDefintion.EntityModel.Database;
using Microsoft.EntityFrameworkCore;

namespace DatabaseDefinition.EntityModel.Database;

[PrimaryKey("Id")]
public class Item
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [ForeignKey("ItemId")]
    public IEnumerable<CartItemLink> CartItemLinks { get; set; }
    [ForeignKey("ItemId")]
    public IEnumerable<ItemPicture> ItemPictures { get; set; }
    [ForeignKey("ItemId")]
    public IEnumerable<ItemPlatformLink> ItemPlatformLinks { get; set; }
}
