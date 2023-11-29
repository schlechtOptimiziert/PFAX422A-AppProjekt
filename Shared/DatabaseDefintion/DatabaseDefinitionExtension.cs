using DatabaseDefinition.EntityModel;
using DatabaseDefinition.EntityModel.Repositories;
using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using DatabaseDefintion.EntityModel.Database;
using DatabaseDefintion.EntityModel.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class DatabaseDefinitionExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddDbContext<AppProjectDbContext>(options =>
            options.UseSqlServer(connectionString));
        serviceCollection.AddDatabaseDeveloperPageExceptionFilter();

        serviceCollection.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<AppProjectDbContext>();

        serviceCollection.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, AppProjectDbContext>();

        serviceCollection.AddAuthentication()
            .AddIdentityServerJwt();

        serviceCollection.AddScoped<IItemRepository, ItemRepository>();
        serviceCollection.AddScoped<IItemPictureRepository, ItemPictureRepository>();
        serviceCollection.AddScoped<ICartRepository, CartRepository>();
        serviceCollection.AddScoped<IPlatformRepositroy, PlatformRepository>();
        serviceCollection.AddScoped<IOrderRepository, OrderRepository>();

        return serviceCollection;
    }
}
