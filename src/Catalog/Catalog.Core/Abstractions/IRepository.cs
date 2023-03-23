using Catalog.Domain.Aggregates.Abstractions;

namespace Catalog.Domain.Abstractions
{
    public interface IRepository<T> where T : class, IAggregateRoot
    {
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task InsertAsync(T obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(object id);
    }
}
