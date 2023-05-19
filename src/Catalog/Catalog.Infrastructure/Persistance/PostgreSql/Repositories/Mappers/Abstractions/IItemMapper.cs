using Catalog.Domain.ItemAggregate;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Models;

namespace Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Mappers.Abstractions
{
    internal interface IItemMapper
    {
        public Item Map(ItemDto itemDto);
    }
}
