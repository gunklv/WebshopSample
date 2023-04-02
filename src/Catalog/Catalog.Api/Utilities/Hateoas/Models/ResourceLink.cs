namespace Catalog.Api.Utilities.Hateoas.Models
{
    public sealed class ResourceLink
    {
        public ResourceLink(string title, string href, string method, object model = null)
        {
            Title = title;
            Href = href;
            Method = method;
            Model = model;
        }

        public string Title { get; set; }
        public string Href { get; set; }
        public string Method { get; set; }
        public object Model { get; set; }
    }
}
