namespace Cart.Api.Api.Models
{
    public class ItemViewModel
    {
        public long ItemId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAltText { get; set; }
    }
}
