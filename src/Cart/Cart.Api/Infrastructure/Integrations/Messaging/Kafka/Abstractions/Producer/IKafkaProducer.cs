using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Producer.Configuration;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Producer.Models;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Producer
{
    internal interface IKafkaProducer<TConfig> where TConfig : KafkaProducerConfiguration
    {
        Task ProduceAsync<TKey, TValue>(KafkaProduceMessage<TKey, TValue> kafkaMessage) where TKey : class where TValue : class;
    }
}
