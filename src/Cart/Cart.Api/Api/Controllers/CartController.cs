using Cart.Api.Api.Mappers.Abstractions;
using Cart.Api.Api.Models;
using Cart.Api.Core.Exceptions;
using Cart.Api.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Cart.Api.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/carts")]
    public class CartController : ControllerBase
    {
        private readonly IItemMapper _itemMapper;
        private readonly ICartService _cartService;

        public CartController(IItemMapper itemMapper, ICartService cartService)
        {
            _itemMapper = itemMapper;
            _cartService = cartService;
        }

        /// <summary>
        /// A [POST] http operation which adds an Item to a specific Cart.
        /// </summary>
        /// <param name="cartKey">The key of the Cart. If the cart does not exist then creates a cart with the given key.</param>
        /// <param name="addItemToCartRequest">The model of the request</param>
        /// <returns>The status code of the operation.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/carts/{cartKey}
        ///     {
        ///        "itemId": 1,
        ///        "name": "name",
        ///        "price": "1",
        ///        "imageUrl": "imageUrl",
        ///        "imageAltText": "imageAltText"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">If the operation was succesful.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexpected issue happened during the operation.</response>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Route("{cartKey}/items")]
        public async Task<IActionResult> AddItemToCartAsync([FromRoute] string cartKey, [FromBody] AddItemToCartRequest addItemToCartRequest)
        {
            var item = _itemMapper.Map(addItemToCartRequest);

            await _cartService.AddItemToCartAsync(cartKey, item);

            return Ok();
        }

        /// <summary>
        /// A [GET] http operation which responses with a Cart model which contains all the Items that were assigned for the Cart.
        /// </summary>
        /// <param name="cartKey">The key of the Cart.</param>
        /// <returns>A Cart model with the assigned Item collection.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/v1/carts/{cartKey}/items
        ///
        /// </remarks>
        /// <response code="200">If the operation was succesful.</response>
        /// <response code="400">If there was not a Cart with the corresponding cartKey.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexpected issue happened during the operation.</response>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Route("{cartKey}/items")]
        public async Task<IActionResult> GetCartItemListAsync([FromRoute] string cartKey)
        {
            try
            {
                var itemCollection = await _cartService.GetCartItemListAsync(cartKey);
                var itemViewModelCollection = itemCollection.Select(x => _itemMapper.Map(x)).ToList();

                var cartViewModel = new GetCartItemListResponse
                {
                    CartKey = cartKey,
                    ItemViewModelCollection = itemViewModelCollection
                };

                return Ok(cartViewModel);
            }
            catch (CartNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// A [GET] http operation which responses all the Items corresponding to the cartKey.
        /// </summary>
        /// <param name="cartKey">The key of the Cart.</param>
        /// <returns>An Item collection corresponding to the cartKey.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/v2/carts/{cartKey}/items
        ///
        /// </remarks>
        /// <response code="200">If the operation was succesful.</response>
        /// <response code="400">If there was not a Cart with the corresponding cartKey.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexpected issue happened during the operation.</response>
        [HttpGet]
        [MapToApiVersion("2.0")]
        [Route("{cartKey}/items")]
        public async Task<IActionResult> GetCartItemListAsyncV2([FromRoute] string cartKey)
        {
            try
            {
                var itemList = await _cartService.GetCartItemListAsync(cartKey);
                var itemViewModelList = itemList.Select(x => _itemMapper.Map(x)).ToList();

                return Ok(itemViewModelList);
            }
            catch (CartNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// A [DELETE] http operation which removes an item from the Cart corresponding to the cartKey. If there are no other items then the Cart will be deleted too.
        /// </summary>
        /// <param name="cartKey">The key of the Cart.</param>
        /// <param name="itemId">The id of the Item.</param>
        /// <returns>The status code of the operation.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/v1/carts/{cartKey}/items/{itemId}
        ///
        /// </remarks>
        /// <response code="200">If the operation was succesful.</response>
        /// <response code="400">If there was not a Cart with the corresponding cartKey.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexpected issue happened during the operation.</response>
        [HttpDelete]
        [MapToApiVersion("1.0")]
        [Route("{cartKey}/items/{itemId}")]
        public async Task<IActionResult> DeleteItemFromCartAsync([FromRoute] string cartKey, [FromRoute] long itemId)
        {
            try
            {
                await _cartService.RemoveItemFromCartAsync(cartKey, itemId);

                return Ok();
            }
            catch(CartNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
