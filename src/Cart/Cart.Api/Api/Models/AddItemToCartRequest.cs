namespace Cart.Api.Api.Models
{
    public class AddItemToCartRequest
    {
        public long ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAltText { get; set; }
    }
}
