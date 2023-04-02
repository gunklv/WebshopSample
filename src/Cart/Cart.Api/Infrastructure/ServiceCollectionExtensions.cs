using Cart.Api.Core.Abstractions.Repositories;
using Cart.Api.Infrastructure.Configurations;
using Cart.Api.Infrastructure.Repositories;

namespace Cart.Api.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<PersistenceConfiguration>(configuration.GetSection("Persistence"));

            serviceCollection.AddSingleton<ICartRepository, CartRepository>();
        }
    }
}
