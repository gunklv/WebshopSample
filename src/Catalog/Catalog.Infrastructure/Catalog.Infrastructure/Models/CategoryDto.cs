namespace Catalog.Infrastructure.Models
{
    internal class CategoryDto
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
