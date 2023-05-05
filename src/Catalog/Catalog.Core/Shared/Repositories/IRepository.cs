using Catalog.Domain.Shared.Aggregates;

namespace Catalog.Domain.Shared.Repositories
{
    public interface IRepository<T> where T : AggregateRoot
    {
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task<T> InsertAsync(T obj);
        Task<T> UpdateAsync(T obj);
        Task DeleteAsync(T obj);
    }
}
