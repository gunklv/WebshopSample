namespace Catalog.Api.Models.Category.ViewModels
{
    public class CategoryViewModel
    {
        public Guid CategoryId { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
