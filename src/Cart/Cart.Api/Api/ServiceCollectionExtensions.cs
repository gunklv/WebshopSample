using Cart.Api.Api.Mappers.Abstractions;
using Cart.Api.Api.Mappers;
using Cart.Api.Api.Services;

namespace Cart.Api.Api
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApi(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IItemMapper, ItemMapper>();

            serviceCollection.AddHostedService<IntegrationEventProcessorService>();
        }
    }
}
