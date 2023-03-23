using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.Mappers.Abstractions;
using Catalog.Infrastructure.Models;

namespace Catalog.Infrastructure.Mappers
{
    internal class ItemMapper : IItemMapper
    {
        public Item Map(ItemDto itemDto, Category category)
        => new Item(itemDto.Id, itemDto.Name, itemDto.Description, itemDto.ImageUrl, itemDto.Price, itemDto.Amount, category);
    }
}
