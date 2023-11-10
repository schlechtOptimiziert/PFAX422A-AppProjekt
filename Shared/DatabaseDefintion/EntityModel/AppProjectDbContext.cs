using DatabaseDefinition.EntityModel.Database;
using Microsoft.EntityFrameworkCore;

namespace DatabaseDefinition.EntityModel;

public class AppProjectDbContext : DbContext
{
    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<ItemPicture> ItemPictures { get; set; }

    public AppProjectDbContext(DbContextOptions options) : base(options)
    { }
}
