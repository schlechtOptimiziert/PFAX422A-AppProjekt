using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabaseDefinition.EntityModel.Database;

[PrimaryKey("Id")]
public class BillingAddress
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Name { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public int Postcode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}
