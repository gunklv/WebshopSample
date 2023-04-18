using Catalog.Domain.ItemAggregate;
using Catalog.Infrastructure.Persistance.Sql.Mappers.Abstractions;
using Catalog.Infrastructure.Persistance.Sql.Models;

namespace Catalog.Infrastructure.Persistance.Sql.Mappers
{
    internal class ItemMapper : IItemMapper
    {
        public Item Map(ItemDto itemDto)
        => new Item(itemDto.Id, itemDto.Name, itemDto.Description, itemDto.ImageUrl, itemDto.Price, itemDto.Amount, itemDto.CategoryId);
    }
}
