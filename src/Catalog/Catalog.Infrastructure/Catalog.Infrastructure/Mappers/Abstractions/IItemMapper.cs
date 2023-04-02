using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.Models;

namespace Catalog.Infrastructure.Mappers.Abstractions
{
    internal interface IItemMapper
    {
        public Item Map(ItemDto itemDto, Category category);
    }
}
