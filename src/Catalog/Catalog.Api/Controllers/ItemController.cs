using Catalog.Api.Attributes;
using Catalog.Api.Exceptions;
using Catalog.Api.Mappers.Abstractions;
using Catalog.Api.Models.Item.Requests;
using Catalog.Application.Item.Commands.DeleteItem;
using Catalog.Application.Item.Queries.GetItem;
using Catalog.Application.Item.Queries.ListItems;
using MediatR;
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
        public async Task<IActionResult> CreateAsync([FromBody]CreateItemRequest createItemRequest)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidRequestException(ModelState);
            }

            var createItemCommand = _itemMapper.Map(createItemRequest);
            await _mediator.Send(createItemCommand);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        [Route("{itemId}")]
        public async Task<IActionResult> GetAsync([FromRoute][NotDefault] long itemId)
        {
            var item = await _mediator.Send(new GetItemQuery { Id = itemId });
            var itemDto = _itemMapper.Map(item);

            return Ok(itemDto);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var items = await _mediator.Send(new ListItemsQuery());
            var itemDtoList = items.Select(item => _itemMapper.Map(item));

            return Ok(itemDtoList);
        }

        [HttpPut]
        [Route("{itemId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute][NotDefault] long itemId, [FromBody] UpdateItemRequest updateItemRequest)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidRequestException(ModelState);
            }

            var updateItemCommand = _itemMapper.Map(itemId, updateItemRequest);
            await _mediator.Send(updateItemCommand);

            return Ok();
        }

        [HttpDelete]
        [Route("{itemId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute][NotDefault] long itemId)
        {
            await _mediator.Send(new DeleteItemCommand { Id = itemId });

            return Ok();
        }
    }
}