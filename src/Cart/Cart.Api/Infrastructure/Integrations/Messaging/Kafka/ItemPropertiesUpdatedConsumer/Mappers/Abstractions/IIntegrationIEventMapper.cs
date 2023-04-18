using Cart.Api.Core.IntegrationEvents.Events.Abstractions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Models;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Mappers.Abstractions
{
    internal interface IIntegrationIEventMapper<in TMessage, out TEvent> where TMessage : KafkaConsumeMessage where TEvent : IntegrationEvent
    {
        TEvent Map(TMessage message);
    }
}
