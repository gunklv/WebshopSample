namespace Catalog.Api.Utilities.Hateoas.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class LinkAttribute : Attribute
    {
        public string LinkedActionName { get; private set; }
        public string IdName { get; private set; }

        public LinkAttribute(string linkedActionName, string idName)
        {
            LinkedActionName = linkedActionName;
            IdName = idName;
        }
    }
}
