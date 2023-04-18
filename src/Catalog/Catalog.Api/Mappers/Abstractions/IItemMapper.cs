using Catalog.Api.Models.Item.Requests;
using Catalog.Api.Models.Item.ViewModels;
using Catalog.Application.Item.Commands.CreateItem;
using Catalog.Application.Item.Commands.UpdateItem;
using Catalog.Application.Item.Queries.ListItems;
using Catalog.Domain.Aggregates;

namespace Catalog.Api.Mappers.Abstractions
{
    public interface IItemMapper
    {
        CreateItemCommand Map(CreateItemRequest createItemRequest);
        UpdateItemCommand Map(long id, UpdateItemRequest updateItemRequest);
        ItemViewModel Map(Item item);
        ListItemsQuery Map(ListItemsQueryRequest listItemsQueryRequest);
    }
}
