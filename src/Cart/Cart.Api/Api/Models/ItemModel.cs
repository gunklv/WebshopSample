namespace Cart.Api.Api.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ImageModel Image { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
