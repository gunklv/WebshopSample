namespace Catalog.Api.Models.Category.Requests
{
    public class UpdateCategoryRequest
    {
        public Guid ParentId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
