using Cart.Api.Api.Mappers.Abstractions;
using Cart.Api.Core.Exceptions;
using Cart.Api.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Cart.Api.Api.Controllers
{
    [ApiController]
    [Route("carts")]
    public class CartController : ControllerBase
    {
        private readonly IItemMapper _itemMapper;
        private readonly ICartService _cartService;

        public CartController(IItemMapper itemMapper, ICartService cartService)
        {
            _itemMapper = itemMapper;
            _cartService = cartService;
        }

        [HttpPost]
        [Route("{cartId}/items/{itemId}")]
        public async Task<IActionResult> AddItemToCartAsync([FromRoute] Guid cartId, [FromRoute] int itemId)
        {
            await _cartService.AddItemToCartAsync(cartId, itemId);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        [Route("{cartId}/items")]
        public async Task<IActionResult> GetCartItemListAsync(Guid cartId)
        {
            try
            {
                var itemList = await _cartService.GetCartItemListAsync(cartId);
                var itemModelList = itemList.Select(x => _itemMapper.Map(x));

                return Ok(itemModelList);
            }
            catch (CartNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{cartId}/items/{itemId}")]
        public async Task<IActionResult> DeleteItemFromCartAsync(Guid cartId, int itemId)
        {
            try
            {
                await _cartService.RemoveItemFromCartAsync(cartId, itemId);

                return Ok();
            }
            catch(CartNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
