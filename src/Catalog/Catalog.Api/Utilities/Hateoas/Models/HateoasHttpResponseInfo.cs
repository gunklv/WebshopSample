using Newtonsoft.Json;

namespace Catalog.Api.Utilities.Hateoas.Models
{
    internal class HateoasHttpResponseInfo
    {
        public object ResponseObject { get; private set; }
        public string ResponseObjectInJson { get; private set; }
        public Dictionary<string, object> ResponseObjectInDictionary { get; private set; }

        public HateoasHttpResponseInfo(object responseObject)
        {
            ResponseObject = responseObject;
            ResponseObjectInJson = JsonConvert.SerializeObject(responseObject);
            ResponseObjectInDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(ResponseObjectInJson);
        }
    }
}
