using Cart.Api.Core.Abstractions.Repositories;
using Cart.Api.Infrastructure.Repositories;

namespace Cart.Api.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICartRepository, CartRepository>();
        }
    }
}
