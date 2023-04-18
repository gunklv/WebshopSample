using Cart.Api.Core.Models;

namespace Cart.Api.Core.Services.Abstractions
{
    public interface ICartService
    {
        Task AddItemToCartAsync(string cartKey, Item item);
        Task<IReadOnlyCollection<Item>> GetCartItemListAsync(string cartKey);
        Task RemoveItemFromCartAsync(string cartKey, long itemId);
    }
}