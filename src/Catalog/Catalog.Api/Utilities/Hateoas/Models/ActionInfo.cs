namespace Catalog.Api.Utilities.Hateoas.Models
{
    internal class ActionInfo
    {
        public IReadOnlyDictionary<string, object> ActionParameterDictionary { get; set; }
        public IReadOnlyCollection<LinkedActionInfo> LinkedActionInfoCollection { get; set; }

        public ActionInfo(IReadOnlyDictionary<string, object> actionParameterDictionary, IReadOnlyCollection<LinkedActionInfo> linkedActionInfoCollection)
        {
            ActionParameterDictionary = actionParameterDictionary;
            LinkedActionInfoCollection = linkedActionInfoCollection;
        }
    }
}
