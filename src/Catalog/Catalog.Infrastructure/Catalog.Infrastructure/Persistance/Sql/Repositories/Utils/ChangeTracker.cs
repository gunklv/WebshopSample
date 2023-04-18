using Catalog.Domain.Shared.Aggregates;

namespace Catalog.Infrastructure.Persistance.Sql.Repositories.Utils
{
    internal class ChangeTracker : IChangeTracker
    {
        private readonly HashSet<AggregateRoot> _changedAggregateRootHashSet = new HashSet<AggregateRoot>();

        public void AddChangedAggregateRoot(AggregateRoot aggregateRoot)
        {
            _changedAggregateRootHashSet.Add(aggregateRoot);
        }

        public IReadOnlyCollection<AggregateRoot> GetChangedAggregateRoots()
        {
            return _changedAggregateRootHashSet;
        }

        public void CleanChangedAggregateRoots()
        {
            _changedAggregateRootHashSet.Clear();
        }
    }
}
