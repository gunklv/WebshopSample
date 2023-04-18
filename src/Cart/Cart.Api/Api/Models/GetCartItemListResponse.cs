namespace Cart.Api.Api.Models
{
    public class GetCartItemListResponse
    {
        public string CartKey { get; set; }
        public IReadOnlyList<ItemViewModel> ItemViewModelCollection { get; set; }
    }
}
