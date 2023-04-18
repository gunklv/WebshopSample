namespace Catalog.Infrastructure.Persistance.Sql.Models
{
    internal class ItemDto
    {
        public int Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public long Amount { get; set; }
    }
}
