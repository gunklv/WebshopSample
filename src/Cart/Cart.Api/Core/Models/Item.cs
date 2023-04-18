namespace Cart.Api.Core.Models
{
    public class Item
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Image Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
