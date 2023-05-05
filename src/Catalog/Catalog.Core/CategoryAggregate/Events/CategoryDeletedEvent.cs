using Catalog.Domain.Shared.Events;

namespace Catalog.Domain.CategoryAggregate.Events
{
    public class CategoryDeletedEvent : DomainEvent
    {
        public Guid CategoryId { get; }

        public CategoryDeletedEvent(Guid categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
