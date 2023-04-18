using Catalog.Domain.ItemAggregate;
using Catalog.Infrastructure.Persistance.Sql.Models;

namespace Catalog.Infrastructure.Persistance.Sql.Mappers.Abstractions
{
    internal interface IItemMapper
    {
        public Item Map(ItemDto itemDto);
    }
}
