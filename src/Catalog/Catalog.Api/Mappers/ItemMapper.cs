using Catalog.Api.Mappers.Abstractions;
using Catalog.Api.Models.Item.Requests;
using Catalog.Api.Models.Item.ViewModels;
using Catalog.Application.Item.Commands.CreateItem;
using Catalog.Application.Item.Commands.UpdateItem;
using Catalog.Application.Item.Queries.ListItems;
using Catalog.Application.Shared.Models;
using Catalog.Domain.ItemAggregate;

namespace Catalog.Api.Mappers
{
    public class ItemMapper : IItemMapper
    {
        public CreateItemCommand Map(CreateItemRequest createItemRequest)
        => new CreateItemCommand
        {
            CategoryId = createItemRequest.CategoryId,
            Name = createItemRequest.Name,
            Description = createItemRequest.Description,
            ImageUrl = createItemRequest.ImageUrl,
            Amount = createItemRequest.Amount,
            Price = createItemRequest.Price
        };

        public UpdateItemCommand Map(long id, UpdateItemRequest updateItemRequest)
        => new UpdateItemCommand
        {
            Id = id,
            CategoryId = updateItemRequest.CategoryId,
            Name = updateItemRequest.Name,
            Description = updateItemRequest.Description,
            ImageUrl = updateItemRequest.ImageUrl,
            Amount = updateItemRequest.Amount,
            Price = updateItemRequest.Price
        };

        public ItemViewModel Map(Item item)
        => new ItemViewModel
        {
            ItemId = item.Id,
            CategoryId = item.CategoryId,
            Name = item.Name,
            Description = item.Description,
            ImageUrl = item.ImageUrl,
            Amount = item.Amount,
            Price = item.Price
        };

        public ListItemsQuery Map(ListItemsQueryRequest listItemsQueryRequest)
        => new ListItemsQuery
        {
            ListItemsFilter = new ListItemsFilter { CategoryId = listItemsQueryRequest.CategoryId},
            PagedQueryParams = new PagedQueryParams { Page = listItemsQueryRequest.Page, Limit = listItemsQueryRequest.Limit }
        };
    }
}
