using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DatabaseDefintion.EntityModel.Database;
using Microsoft.EntityFrameworkCore;

namespace DatabaseDefinition.EntityModel.Database;

[PrimaryKey("Id")]
public class Order
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; }
    [ForeignKey("BillingAddress")]
    public long BillingAddressId { get; set; }

    public DateTime Date { get; set; }

    public ApplicationUser User { get; set; }
    public BillingAddress BillingAddress { get; set; }
    [ForeignKey("OrderId")]
    public IEnumerable<OrderPosition> Positions { get; set; }
}
