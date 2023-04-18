namespace Cart.Api.Core.Models
{
    public class Cart
    {
        public string Key { get; set; }
        public List<Item> ItemList { get; set; } = new();
    }
}
