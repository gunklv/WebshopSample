using Catalog.IntegrationEventSenderHost.Core.Models;

namespace Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.CatalogIntegrationEventProducer
{
    internal interface ICatalogIntegrationEventMessageProducer
    {
        Task PublishEvent(IntegrationEvent integrationEvent);
    }
}
