using Catalog.Api.Utilities.Hateoas.Services;
using Catalog.Api.Utilities.Hateoas.Services.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Catalog.Api.Utilities.Hateoas
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHateoas(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IHateoasContextService, HateoasContextService>();

            serviceCollection.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            serviceCollection.AddScoped(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
        }
    }
}
