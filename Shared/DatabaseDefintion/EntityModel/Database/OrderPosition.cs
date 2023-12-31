﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseDefinition.EntityModel.Database;

[PrimaryKey("Id")]
public class OrderPosition
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [ForeignKey("Item")]
    public long ItemId { get; set; }
    [ForeignKey("Order")]
    public long OrderId { get; set; }
    public string Name { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    public int Amount { get; set; }

    public Item Item { get; set; }
    public Order Order { get; set; }

    //Empty constructor for EFCore
    public OrderPosition() { }

    public OrderPosition(Item item, long orderId, int amount)
    {
        ItemId = item.Id;
        Name = item.Name;
        Price = item.Price;
        OrderId = orderId;
        Amount = amount;
    }
}
