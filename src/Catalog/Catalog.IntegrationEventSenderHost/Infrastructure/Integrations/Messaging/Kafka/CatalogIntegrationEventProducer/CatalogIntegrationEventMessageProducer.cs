using Catalog.IntegrationEventSenderHost.Core.Models;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Models;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Producer;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.CatalogIntegrationEventProducer.Configuration;
using Newtonsoft.Json;

namespace Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.CatalogIntegrationEventProducer
{
    internal class CatalogIntegrationEventMessageProducer : ICatalogIntegrationEventMessageProducer
    {
        private readonly IKafkaProducer<CatalogIntegrationEventProducerConfiguration> _catalogIntegrationEventMessageProducer;

        public CatalogIntegrationEventMessageProducer(IKafkaProducer<CatalogIntegrationEventProducerConfiguration> catalogIntegrationEventMessageProducer)
        {
            _catalogIntegrationEventMessageProducer = catalogIntegrationEventMessageProducer;
        }

        public async Task PublishEvent(IntegrationEvent integrationEvent)
        {
            var kafkaMessage = new KafkaMessage<string, string>
            {
                Payload = JsonConvert.SerializeObject(integrationEvent.Payload),
                Headers = new Dictionary<string, string>
                    {
                        { "EventId", integrationEvent.EventId.ToString() },
                        { "EventType", integrationEvent.EventType }
                    },
                Key = integrationEvent.EventId.ToString()
            };

            await _catalogIntegrationEventMessageProducer.ProduceAsync(kafkaMessage);
        }
    }
}
