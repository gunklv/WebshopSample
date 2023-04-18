using Cart.Api.Core.IntegrationEvents.Events.Abstractions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Models;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Abstractions
{
    public interface IIntegrationEventConsumer<TMessage, TEvent> : IIntegrationEventConsumer
        where TMessage : IntegrationEventConsumeMessage
        where TEvent : IntegrationEvent
    {
    }

    public interface IIntegrationEventConsumer
    {
        Task ConsumeAsync(CancellationToken cancellationToken);
    }
}
