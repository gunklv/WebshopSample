using Catalog.Application.Shared.Mappers.Abstractions;
using Catalog.Application.Shared.Models.Abstractions;
using Catalog.Domain.ItemAggregate.Events;
using Catalog.Domain.Shared.Events;

namespace Catalog.Application.Item.IntegrationEvents
{
    internal class ItemIntegrationEventMapper : IIntegrationEventMapper
    {
        public IntegrationEvent MapDomainEvent(DomainEvent domainEvent)
        {
            return domainEvent switch
            {
                ItemPropertiesUpdatedEvent @event => new ItemPropertiesUpdatedIntegrationEvent(
                    @event.Item.Id, @event.Item.CategoryId, @event.Item.Name, @event.Item.Description, @event.Item.Amount, @event.Item.Price, @event.Item.ImageUrl),
                { } => null
            };
        }
    }
}
