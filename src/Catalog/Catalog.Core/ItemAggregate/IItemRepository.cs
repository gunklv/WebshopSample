using Catalog.Domain.Shared.Repositories;

namespace Catalog.Domain.ItemAggregate
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IReadOnlyCollection<Item>> GetAllAsync(Action<ItemFilter> itemFilterAction, int? page, int? limit);
    }

    public class ItemFilter
    {
        public Guid? CategoryId { get; set; }
    }
}
