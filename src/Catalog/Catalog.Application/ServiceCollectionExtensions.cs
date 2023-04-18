using Catalog.Application.Item.IntegrationEvents;
using Catalog.Application.Shared.Mappers.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Catalog.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(c => c.RegisterServicesFromAssemblies(Assembly.GetAssembly(typeof(ServiceCollectionExtensions))));

            serviceCollection.AddSingleton<IIntegrationEventMapper, ItemIntegrationEventMapper>();
        }
    }
}
