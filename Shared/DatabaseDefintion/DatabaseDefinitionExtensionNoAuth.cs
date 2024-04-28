using DatabaseDefinition.EntityModel;
using DatabaseDefinition.EntityModel.Repositories;
using DatabaseDefinition.EntityModel.Repositories.Interfaces;
using DatabaseDefintion.EntityModel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class DatabaseDefinitionExtensionNoAuth
{
    public static IServiceCollection AddDatabaseNoAuth(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddDbContext<AppProjectDbContext>(options =>
            options.UseSqlServer(connectionString));
        serviceCollection.AddDatabaseDeveloperPageExceptionFilter();

        serviceCollection.AddScoped<IItemRepository, ItemRepository>();
        serviceCollection.AddScoped<IItemPictureRepository, ItemPictureRepository>();
        serviceCollection.AddScoped<ICartRepository, CartRepository>();
        serviceCollection.AddScoped<IPlatformRepositroy, PlatformRepository>();
        serviceCollection.AddScoped<IOrderRepository, OrderRepository>();

        return serviceCollection;
    }
}
