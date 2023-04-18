using Cart.Api.Core.IntegrationEvents.Events;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Mappers.Abstractions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Models;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Mappers
{
    internal class ItemPropertiesUpdatedEventMapper : IIntegrationIEventMapper<ItemPropertiesUpdatedMessage, ItemPropertiesUpdatedIntegrationEvent>
    {
        public ItemPropertiesUpdatedIntegrationEvent Map(ItemPropertiesUpdatedMessage message)
        {
            var itemId = long.Parse(message.ItemId);
            var categoryId = Guid.Parse(message.CategoryId);
            var amount = long.Parse(message.Amount);
            var price = decimal.Parse(message.Price);

            return new ItemPropertiesUpdatedIntegrationEvent(itemId, categoryId, message.Name, message.Description, amount, price, message.ImageUrl);
        }
    }
}
