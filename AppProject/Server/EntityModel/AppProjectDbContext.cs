using AppProject.Server.EntityModel.Database;
using Microsoft.EntityFrameworkCore;

namespace AppProject.Server.EntityModel;

public class AppProjectDbContext : DbContext
{
    public virtual DbSet<Item> Items { get; set; }

    public AppProjectDbContext(DbContextOptions<AppProjectDbContext> options)
        : base(options)
    { }
}
