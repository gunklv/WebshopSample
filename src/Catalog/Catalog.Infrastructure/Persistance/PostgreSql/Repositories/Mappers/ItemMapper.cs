using Catalog.Domain.ItemAggregate;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Mappers.Abstractions;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Models;

namespace Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Mappers
{
    internal class ItemMapper : IItemMapper
    {
        public Item Map(ItemDto itemDto)
        => new Item(itemDto.Id, itemDto.Name, itemDto.Description, itemDto.ImageUrl, itemDto.Price, itemDto.Amount, itemDto.CategoryId);
    }
}
