namespace Catalog.Api.Utilities.Hateoas.Models
{
    internal class HateoasHttpRequestInfo
    {
        public ControllerInfo ControllerInfo { get; set; }

        public HateoasHttpRequestInfo(ControllerInfo controllerInfo)
        {
            ControllerInfo = controllerInfo;
        }
    }
}
