using Catalog.Domain.Abstractions.Filters;
using Catalog.Domain.Aggregates;
using System.Linq.Expressions;

namespace Catalog.Domain.Abstractions
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IReadOnlyCollection<Item>> GetAllAsync(Action<ItemFilter> itemFilterAction, int? page, int? limit);
    }
}
