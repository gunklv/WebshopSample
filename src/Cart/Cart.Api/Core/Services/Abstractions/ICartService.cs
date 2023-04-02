using Domain = Cart.Api.Core.Models;

namespace Cart.Api.Core.Services.Abstractions
{
    public interface ICartService
    {
        Task AddItemToCartAsync(Guid cartId, int itemId);
        Task<IReadOnlyCollection<Domain.Item>> GetCartItemListAsync(Guid cartId);
        Task RemoveItemFromCartAsync(Guid cartId, int itemId);
    }
}