using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(c => c.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        }
    }
}
