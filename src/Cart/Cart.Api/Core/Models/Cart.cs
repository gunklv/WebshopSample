namespace Cart.Api.Core.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public List<Item> ItemList { get; set; } = new();
    }
}
