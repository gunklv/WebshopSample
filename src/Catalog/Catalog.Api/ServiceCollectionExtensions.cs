using Catalog.Api.Mappers;
using Catalog.Api.Mappers.Abstractions;
using Catalog.Api.Utilities.Hateoas;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Catalog.Api
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApi(this IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            serviceCollection.AddFluentValidationAutoValidation();
            serviceCollection.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic));

            serviceCollection.AddSingleton<IItemMapper, ItemMapper>();
            serviceCollection.AddScoped<ICategoryMapper, CategoryMapper>();

            serviceCollection.AddHateoas();
        }
    }
}
