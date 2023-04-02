using Catalog.Api.Models.Item.Requests;
using Catalog.Api.Models.Item.ViewModels;
using Catalog.Application.Item.Commands.CreateItem;
using Catalog.Application.Item.Commands.UpdateItem;
using Catalog.Domain.Aggregates;

namespace Catalog.Api.Mappers.Abstractions
{
    public interface IItemMapper
    {
        CreateItemCommand Map(CreateItemRequest createItemRequest);
        UpdateItemCommand Map(long id, UpdateItemRequest updateItemRequest);
        ItemViewModel Map(Item item);
    }
}
