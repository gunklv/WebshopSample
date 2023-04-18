using Domain = Cart.Api.Core.Models;

namespace Cart.Api.Core.Abstractions.Repositories
{
    public interface ICartRepository
    {
        Task InsertCartAsync(Domain.Cart cart);

        Task<Domain.Cart> GetCartAsync(string cartKey);

        Task UpdateCartAsync(Domain.Cart cart);

        Task DeleteCartAsync(string cartKey);
    }
}