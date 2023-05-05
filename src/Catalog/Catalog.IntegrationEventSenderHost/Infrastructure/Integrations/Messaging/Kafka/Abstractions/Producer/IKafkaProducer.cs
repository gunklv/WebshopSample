using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Models;

namespace Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Producer
{
    internal interface IKafkaProducer<TConfig> where TConfig : KafkaProducerConfiguration
    {
        Task ProduceAsync<TKey, TValue>(KafkaMessage<TKey, TValue> kafkaMessage) where TKey : class where TValue : class;
    }
}
