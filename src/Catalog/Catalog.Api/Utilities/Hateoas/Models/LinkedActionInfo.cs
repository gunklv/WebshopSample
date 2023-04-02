namespace Catalog.Api.Utilities.Hateoas.Models
{
    internal class LinkedActionInfo
    {
        public string LinkedActionHttpMethodType { get; set; }
        public string LinkedActionIdName { get; set; }
        public string LinkedActionRouteName { get; set; }
        public object ModelObject { get; private set; }

        public LinkedActionInfo(
            string linkedActionHttpMethodType,
            string linkedActionIdName,
            string linkedActionRouteName,
            object modelObject)
        {
            LinkedActionHttpMethodType = linkedActionHttpMethodType;
            LinkedActionIdName = linkedActionIdName;
            LinkedActionRouteName = linkedActionRouteName;
            ModelObject = modelObject;
        }
    }
}
