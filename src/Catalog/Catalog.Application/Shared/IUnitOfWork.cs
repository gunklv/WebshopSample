using Catalog.Domain.ItemAggregate;
using Catalog.Domain.Shared.Repositories;

namespace Catalog.Application.Shared
{
    public interface IUnitOfWork : IDisposable
    {
        IItemRepository ItemRepository { get; }
        IRepository<Domain.CategoryAggregate.Category> CategoryRepository { get; }

        Task BeginAsync();
        Task CommitAsync();
        void Rollback();
    }
}
