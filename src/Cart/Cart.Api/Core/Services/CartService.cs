using Cart.Api.Core.Abstractions.Repositories;
using Cart.Api.Core.Models.Exceptions;
using Cart.Api.Core.Services.Abstractions;
using Domain = Cart.Api.Core.Models;

namespace Cart.Api.Core.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task AddItemToCartAsync(Guid cartId, int itemId)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);

            if(cart == null)
            {
                cart = new Domain.Cart { Id = cartId };
                await _cartRepository.InsertCartAsync(cart);
            }

            var cartItem = cart.ItemList.FirstOrDefault(x => x.Id == itemId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                // Here could come a logic to collect Item data
                var item = new Domain.Item { Id = itemId, Name = "DummyItem", Price = 1000, Quantity = 1, Image = new Domain.Image { AltText = "DummyAltText", Url = "DummyUrl" } };

                cart.ItemList.Add(item);
            }

            await _cartRepository.UpdateCartAsync(cart);
        }

        public async Task<IReadOnlyCollection<Domain.Item>> GetCartItemListAsync(Guid cartId)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);

            if (cart == null)
            {
                throw new CartNotFoundException($"Cart with id: {cartId} do not exist.");
            }

            return cart.ItemList;
        }

        public async Task RemoveItemFromCartAsync(Guid cartId, int itemId)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);

            if(cart == null)
            {
                throw new CartNotFoundException($"Cart with id: {cartId} do not exist.");
            }

            var removableItem = cart.ItemList.FirstOrDefault(x => x.Id == itemId);

            if(removableItem == null)
                return;

            if(removableItem.Quantity > 1)
            {
                removableItem.Quantity--;
            }
            else
            {
                cart.ItemList.Remove(removableItem);
            }

            if (!cart.ItemList.Any())
            {
                await _cartRepository.DeleteCartAsync(cartId);
            }
            else
            {
                await _cartRepository.UpdateCartAsync(cart);
            }
        }
    }
}
