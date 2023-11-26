using DatabaseDefinition.EntityModel.Database;
using DatabaseDefintion.EntityModel.Database;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DatabaseDefinition.EntityModel;

public class AppProjectDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<ItemPicture> ItemPictures { get; set; }
    public virtual DbSet<CartItemLink> CartItemLinks { get; set; }
    public virtual DbSet<Platform> Platforms { get; set; }
    public virtual DbSet<ItemPlatformLink> ItemPlatformLinks { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderPosition> OrderPositions { get; set; }
    public virtual DbSet<BillingAddress> BillingAddresses { get; set; }

    public AppProjectDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    { }
}
