using DatabaseDefinition.EntityModel;
using DatabaseDefinition.EntityModel.Repositories;
using DatabaseDefinition.EntityModel.Repositories.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

public static class DatabaseDefinitionExtension
{

    public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, string connectionString)
    {

        serviceCollection.AddSqlServer<AppProjectDbContext>(connectionString);

        serviceCollection.AddScoped<IItemRepository, ItemRepository>();
        serviceCollection.AddScoped<IItemPictureRepository, ItemPictureRepository>();

        return serviceCollection;
    }
}
