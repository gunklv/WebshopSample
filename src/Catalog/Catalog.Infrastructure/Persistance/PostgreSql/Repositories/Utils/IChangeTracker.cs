using Catalog.Domain.Shared.Aggregates;

namespace Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Utils
{
    internal interface IChangeTracker
    {
        public void AddChangedAggregateRoot(AggregateRoot aggregateRoot);
        public IReadOnlyCollection<AggregateRoot> GetChangedAggregateRoots();
    }
}
