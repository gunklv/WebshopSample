using Catalog.Domain.Abstractions;
using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.Configurations;
using Catalog.Infrastructure.Mappers;
using Catalog.Infrastructure.Mappers.Abstractions;
using Catalog.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddOptions<PersistenceConfiguration>().Bind(configuration.GetSection("Persistence"));

            serviceCollection.AddSingleton<ICategoryMapper, CategoryMapper>();
            serviceCollection.AddSingleton<IItemMapper, ItemMapper>();

            serviceCollection.AddScoped<IItemRepository, ItemRepository>();
            serviceCollection.AddScoped<IRepository<Category>, CategoryRepository>();
        }
    }
}
