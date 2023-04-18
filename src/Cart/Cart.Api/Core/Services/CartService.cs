using Cart.Api.Core.Abstractions.Repositories;
using Cart.Api.Core.Exceptions;
using Cart.Api.Core.Models;
using Cart.Api.Core.Services.Abstractions;
using Cart.Api.Core.Validators.Abstractions;
using Domain = Cart.Api.Core.Models;

namespace Cart.Api.Core.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IDomainValidator<Item> _itemValidator;

        public CartService(IDomainValidator<Item> itemValidator,ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            _itemValidator = itemValidator;
        }

        public async Task AddItemToCartAsync(string cartKey, Item item)
        {
            await _itemValidator.EnsureValidityAsync(item);

            var cart = await _cartRepository.GetCartAsync(cartKey);

            if(cart == null)
            {
                cart = new Domain.Cart { Key = cartKey };
                await _cartRepository.InsertCartAsync(cart);
            }

            var cartItem = cart.ItemList.FirstOrDefault(x => x.Id == item.Id);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cart.ItemList.Add(item);
            }

            await _cartRepository.UpdateCartAsync(cart);
        }

        public async Task<IReadOnlyCollection<Item>> GetCartItemListAsync(string cartKey)
        {
            var cart = await _cartRepository.GetCartAsync(cartKey);

            if (cart == null)
            {
                throw new CartNotFoundException($"Cart with key: {cartKey} do not exist.");
            }

            return cart.ItemList;
        }

        public async Task RemoveItemFromCartAsync(string cartKey, long itemId)
        {
            var cart = await _cartRepository.GetCartAsync(cartKey);

            if(cart == null)
            {
                throw new CartNotFoundException($"Cart with key: {cartKey} do not exist.");
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
                await _cartRepository.DeleteCartAsync(cartKey);
            }
            else
            {
                await _cartRepository.UpdateCartAsync(cart);
            }
        }
    }
}
