using Catalog.Domain.Shared.Aggregates;

namespace Catalog.Infrastructure.Persistance.Sql.Repositories.Utils
{
    internal interface IChangeTracker
    {
        public void AddChangedAggregateRoot(AggregateRoot aggregateRoot);
        public IReadOnlyCollection<AggregateRoot> GetChangedAggregateRoots();
    }
}
