using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DatabaseDefintion.EntityModel.Database;

public class ApplicationUser : IdentityUser
{
    [ForeignKey("UserId")]
    public IEnumerable<CartItemLink> CartItemLinks { get; set; }
}