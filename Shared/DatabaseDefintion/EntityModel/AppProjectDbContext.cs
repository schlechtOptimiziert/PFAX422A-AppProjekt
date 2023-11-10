using Microsoft.EntityFrameworkCore;
using Shared.DatabaseDefinition.EntityModel.Database;

namespace Shared.DatabaseDefinition.EntityModel;

public class AppProjectDbContext : DbContext
{
    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<ItemPicture> ItemPictures { get; set; }

    public AppProjectDbContext(DbContextOptions options) : base(options)
    { }
}
