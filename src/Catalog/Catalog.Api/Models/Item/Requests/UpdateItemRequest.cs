namespace Catalog.Api.Models.Item.Requests
{
    public class UpdateItemRequest
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Amount { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
