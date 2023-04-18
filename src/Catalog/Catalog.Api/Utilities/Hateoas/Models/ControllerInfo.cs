namespace Catalog.Api.Utilities.Hateoas.Models
{
    internal class ControllerInfo
    {
        public ActionInfo ActionInfo { get; set; }

        public ControllerInfo(ActionInfo actionInfo)
        {
            ActionInfo = actionInfo;
        }
    }
}
