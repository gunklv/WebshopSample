using Catalog.Application.Shared;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Mappers;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Mappers.Abstractions;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Reflection;

namespace Catalog.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddMediatR(c => c.RegisterServicesFromAssemblies(Assembly.GetAssembly(typeof(ServiceCollectionExtensions))));

            serviceCollection.AddTransient((s) => new NpgsqlConnection(configuration.GetValue<string>("Persistence:ConnectionString")));

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddSingleton<IChangeTracker, ChangeTracker>();

            serviceCollection.AddSingleton<ICategoryMapper, CategoryMapper>();
            serviceCollection.AddSingleton<IItemMapper, ItemMapper>();
        }
    }
}
