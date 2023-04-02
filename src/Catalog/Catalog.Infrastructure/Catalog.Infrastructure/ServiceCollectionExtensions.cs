using Catalog.Domain.Abstractions;
using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.Mappers;
using Catalog.Infrastructure.Mappers.Abstractions;
using Catalog.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICategoryMapper, CategoryMapper>();
            serviceCollection.AddSingleton<IItemMapper, ItemMapper>();

            serviceCollection.AddScoped<IRepository<Item>, ItemRepository>();
            serviceCollection.AddScoped<IRepository<Category>, CategoryRepository>();
        }
    }
}
