using Catalog.Api.Attributes;
using Catalog.Api.Exceptions;
using Catalog.Api.Mappers.Abstractions;
using Catalog.Api.Models.Item.Requests;
using Catalog.Api.Utilities.Hateoas.Attributes;
using Catalog.Application.Item.Commands.DeleteItem;
using Catalog.Application.Item.Queries.GetItem;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemController : ControllerBase
    {
        private readonly IItemMapper _itemMapper;
        private readonly IMediator _mediator;

        public ItemController(IItemMapper itemMapper, IMediator mediator)
        {
            _itemMapper= itemMapper;
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("", Name = "CreateItem")]
        [Link(nameof(GetItemAsync), "itemId")]
        [Link(nameof(UpdateItemAsync), "itemId")]
        [Link(nameof(DeleteItemAsync), "itemId")]
        public async Task<IActionResult> CreateItemAsync([FromBody]CreateItemRequest createItemRequest)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidRequestException(ModelState);
            }

            var createItemCommand = _itemMapper.Map(createItemRequest);
            var item =await _mediator.Send(createItemCommand);

            var itemViewModel = _itemMapper.Map(item);

            return StatusCode(StatusCodes.Status201Created, itemViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Buyer")]
        [Route("{itemId}", Name = "GetItem")]
        [Link(nameof(GetItemAsync), "itemId")]
        [Link(nameof(UpdateItemAsync), "itemId")]
        [Link(nameof(DeleteItemAsync), "itemId")]
        public async Task<IActionResult> GetItemAsync([FromRoute][NotDefault] long itemId)
        {
            var item = await _mediator.Send(new GetItemQuery { Id = itemId });
            var itemDto = _itemMapper.Map(item);

            return Ok(itemDto);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Buyer")]
        public async Task<IActionResult> ListItemsAsync([FromQuery] ListItemsQueryRequest listItemsQueryRequest)
        {
            var listItemsQuery = _itemMapper.Map(listItemsQueryRequest);

            var items = await _mediator.Send(listItemsQuery);
            var itemViewModelCollection = items.Select(item => _itemMapper.Map(item));

            return Ok(itemViewModelCollection);
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        [Route("{itemId}", Name = "UpdateItem")]
        [Link(nameof(GetItemAsync), "itemId")]
        [Link(nameof(UpdateItemAsync), "itemId")]
        [Link(nameof(DeleteItemAsync), "itemId")]
        public async Task<IActionResult> UpdateItemAsync([FromRoute][NotDefault] long itemId, [FromBody] UpdateItemRequest updateItemRequest)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidRequestException(ModelState);
            }

            var updateItemCommand = _itemMapper.Map(itemId, updateItemRequest);
            var item = await _mediator.Send(updateItemCommand);

            var itemViewModel = _itemMapper.Map(item);

            return Ok(itemViewModel);
        }

        [HttpDelete]
        [Authorize(Roles = "Manager")]
        [Route("{itemId}", Name = "DeleteItem")]
        [Link(nameof(CreateItemAsync), "")]
        public async Task<IActionResult> DeleteItemAsync([FromRoute][NotDefault] long itemId)
        {
            await _mediator.Send(new DeleteItemCommand { Id = itemId });

            return Ok(new { });
        }
    }
}