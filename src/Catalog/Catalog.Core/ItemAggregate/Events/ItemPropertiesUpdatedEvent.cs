using Catalog.Domain.Shared.Events;

namespace Catalog.Domain.ItemAggregate.Events
{
    public class ItemPropertiesUpdatedEvent : DomainEvent
    {
        public Item Item { get; }

        public ItemPropertiesUpdatedEvent(Item item)
        {
            Item = item;
        }
    }
}
