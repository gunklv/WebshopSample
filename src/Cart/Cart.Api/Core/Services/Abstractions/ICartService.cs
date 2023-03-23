using Cart.Api.Core.Models;
using Domain = Cart.Api.Core.Models;

namespace Cart.Api.Core.Services.Abstractions
{
    public interface ICartService
    {
        Task AddItemToCartAsync(Guid cartId, int itemId);
        Task<IReadOnlyCollection<Item>> GetCartItemListAsync(Guid cartId);
        Task RemoveItemFromCartAsync(Guid cartId, int itemId);
    }
}