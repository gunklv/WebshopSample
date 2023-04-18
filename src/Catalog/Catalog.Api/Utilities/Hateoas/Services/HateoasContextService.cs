using Catalog.Api.Utilities.Hateoas.Attributes;
using Catalog.Api.Utilities.Hateoas.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Catalog.Api.Utilities.Hateoas.Services.Abstractions;
using System.Dynamic;

namespace Catalog.Api.Utilities.Hateoas.Services
{
    internal class HateoasContextService : IHateoasContextService
    {
        public HateoasHttpRequestInfo GetHateoasHttpRequestInfo(HttpContext httpContext, IReadOnlyCollection<MethodInfo> controllerActionMethodInfoCollection)
        {
            var httpRequestRouteData = httpContext.GetRouteData();

            var controllerName = httpRequestRouteData.Values["controller"].ToString();

            var actionName = httpRequestRouteData.Values["action"].ToString();
            var actionMethodInfo = controllerActionMethodInfoCollection.FirstOrDefault(x => x.DeclaringType.Name.Contains(controllerName) && x.Name.Contains(actionName));
            var actionParameterCollection = httpRequestRouteData.Values.Where(x => x.Key != "controller" && x.Key != "action").ToDictionary(x => x.Key, x => x.Value);

            var linkedActionInfoCollection = new List<LinkedActionInfo>();
            var linkedActionAttributes = actionMethodInfo.CustomAttributes.Where(x => x.AttributeType == typeof(LinkAttribute));

            foreach (var linkedActionAttribute in linkedActionAttributes)
            {
                var linkedActionName = linkedActionAttribute.ConstructorArguments[0].Value.ToString();
                var linkedActionMethodInfo = controllerActionMethodInfoCollection.FirstOrDefault(x => x.DeclaringType.Name.Contains(controllerName) && linkedActionName == x.Name);

                var linkedActionHttpMethodAttributeType = linkedActionMethodInfo.CustomAttributes.FirstOrDefault(x => x.AttributeType.IsSubclassOf(typeof(HttpMethodAttribute))).AttributeType;
                var linkedActionHttpMethodType = GetHttpMethodAttributeTypeString(linkedActionHttpMethodAttributeType);

                var linkedActionIdName = linkedActionAttribute.ConstructorArguments[1].Value.ToString();
                
                var linkedActionRouteName = linkedActionMethodInfo.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(RouteAttribute)).NamedArguments[0].TypedValue.Value.ToString();
                
                var linkedActionModelParameterInfo = linkedActionMethodInfo.GetParameters().FirstOrDefault(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(FromBodyAttribute)));

                object linkedActionModelObject = null;
                if (linkedActionModelParameterInfo != null)
                    linkedActionModelObject = GetModelObjectFromParameterInfo(linkedActionModelParameterInfo);
                
                var linkedAction = new LinkedActionInfo(linkedActionHttpMethodType, linkedActionIdName, linkedActionRouteName, linkedActionModelObject);
                linkedActionInfoCollection.Add(linkedAction);
            }

            var actionInfo = new ActionInfo(actionParameterCollection, linkedActionInfoCollection);
            var controllerInfo = new ControllerInfo(actionInfo);

            return new HateoasHttpRequestInfo(controllerInfo);
        }

        public HateoasHttpResponseInfo GetHateoasHttpResponseInfo(object responseObject)
        {
            return new HateoasHttpResponseInfo(responseObject);
        }

        private object GetModelObjectFromParameterInfo(ParameterInfo parameterInfo)
        {
            ICollection<KeyValuePair<string, object>> dynamicParameterCollectionObject = new ExpandoObject();

            var properties = parameterInfo.ParameterType.GetProperties();

            foreach (var p in properties)
            {
                var propertyType = p.PropertyType;
                var propertyTypeName = propertyType.Name;

                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                    propertyTypeName = $"{propertyType.Name}?";
                }

                ((IDictionary<string, object>)dynamicParameterCollectionObject)[p.Name] = propertyTypeName;
            }

            return dynamicParameterCollectionObject;
        }

        public static string GetHttpMethodAttributeTypeString(Type httpMethodAttributeType)
        {
            string httpMethod = null;
            if (httpMethodAttributeType == typeof(HttpPostAttribute))
            {
                httpMethod = "POST";
            }
            if (httpMethodAttributeType == typeof(HttpGetAttribute))
            {
                httpMethod = "GET";
            }
            if (httpMethodAttributeType == typeof(HttpPutAttribute))
            {
                httpMethod = "PUT";
            }
            if (httpMethodAttributeType == typeof(HttpDeleteAttribute))
            {
                httpMethod = "DELETE";
            }
            if (httpMethodAttributeType == typeof(HttpOptionsAttribute))
            {
                httpMethod = "OPTIONS";
            }
            if (httpMethodAttributeType == typeof(HttpPatchAttribute))
            {
                httpMethod = "PATCH";
            }
            return httpMethod;
        }
    }
}
