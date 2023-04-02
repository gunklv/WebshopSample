using Cart.Api.Core.Services.Abstractions;
using Cart.Api.Core.Services;
using Cart.Api.Core.Validators.Abstractions;
using Cart.Api.Core.Validators;
using Cart.Api.Core.Models;

namespace Cart.Api.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCore(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDomainValidator<Item>, ItemValidator>();
            serviceCollection.AddScoped<ICartService, CartService>();
        }
    }
}
