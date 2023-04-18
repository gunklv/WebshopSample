using Catalog.Api.Utilities.Hateoas.Models;
using System.Reflection;

namespace Catalog.Api.Utilities.Hateoas.Services.Abstractions
{
    internal interface IHateoasContextService
    {
        HateoasHttpRequestInfo GetHateoasHttpRequestInfo(HttpContext httpContext, IReadOnlyCollection<MethodInfo> controllerActionMethodInfoCollection);
        HateoasHttpResponseInfo GetHateoasHttpResponseInfo(object responseObject);
    }
}
