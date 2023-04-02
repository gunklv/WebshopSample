using Catalog.Api.Utilities.Hateoas.Models;
using Catalog.Api.Utilities.Hateoas.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Catalog.Api.Utilities.Hateoas
{
    public class HalJsonMediaTypeFormatter : TextOutputFormatter
    {
        private static readonly List<MethodInfo> AllControllerActionMethodInfo =
            typeof(Program).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ControllerBase)) && !t.IsAbstract)
            .SelectMany(x => x.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)).ToList();

        public HalJsonMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/hal+json"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            return !typeof(IEnumerable<object>).IsAssignableFrom(type);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var httpContext = context.HttpContext;
            var serviceProvider = httpContext.RequestServices;

            var hateoasContextService = serviceProvider.GetService<IHateoasContextService>();

            var hateoasHttpRequestInfo = hateoasContextService.GetHateoasHttpRequestInfo(httpContext, AllControllerActionMethodInfo);
            var hateoasHttpResponseInfo = hateoasContextService.GetHateoasHttpResponseInfo(context.Object);

            // Concatenate the request paramaters and response model parameters into one collection in order to be able to find the link-id between the links
            var parameterKeyValuePairCollection = new List<KeyValuePair<string, object>>();
            parameterKeyValuePairCollection.AddRange(hateoasHttpRequestInfo.ControllerInfo.ActionInfo.ActionParameterDictionary);
            parameterKeyValuePairCollection.AddRange(hateoasHttpResponseInfo.ResponseObjectInDictionary);

            var urlHelper = serviceProvider.GetService<IUrlHelper>();

            var resourceLinkCollection = new List<ResourceLink>();
            foreach (var linkedActionInfo in hateoasHttpRequestInfo.ControllerInfo.ActionInfo.LinkedActionInfoCollection)
            {
                // if there was assigned any link-id name to the LinkAttribute then extracting idObject from request and response parameters
                var dynamicIdObject = new ExpandoObject();
                if (!string.IsNullOrEmpty(linkedActionInfo.LinkedActionIdName))
                {
                    var idKeyValuePair = parameterKeyValuePairCollection.FirstOrDefault(parameterKvp => parameterKvp.Key.Equals(linkedActionInfo.LinkedActionIdName, StringComparison.OrdinalIgnoreCase));
                    dynamicIdObject.TryAdd(idKeyValuePair.Key, idKeyValuePair.Value);
                }

                resourceLinkCollection.Add(
                    new ResourceLink(
                        title: linkedActionInfo.LinkedActionRouteName,
                        href: urlHelper.Link(linkedActionInfo.LinkedActionRouteName, dynamicIdObject),
                        method: linkedActionInfo.LinkedActionHttpMethodType,
                        model: linkedActionInfo.ModelObject)
                    );
            }

            dynamic dynamicPayload = JsonConvert.DeserializeObject<ExpandoObject>(hateoasHttpResponseInfo.ResponseObjectInJson);
            ((IDictionary<string, object>)dynamicPayload)["_links"] = resourceLinkCollection;

            await httpContext.Response.WriteAsJsonAsync(
                (object)dynamicPayload, new JsonSerializerOptions() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
        }
    }
}
