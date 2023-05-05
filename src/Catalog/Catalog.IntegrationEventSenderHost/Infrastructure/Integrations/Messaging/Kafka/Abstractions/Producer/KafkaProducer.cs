using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text;

namespace Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Producer
{
    internal class KafkaProducer<TConfig> : IKafkaProducer<TConfig> where TConfig : KafkaProducerConfiguration
    {
        private readonly IOptionsMonitor<TConfig> _options;

        public KafkaProducer(IOptionsMonitor<TConfig> options)
        {
            _options = options;
        }

        private IProducer<TKey, TValue> CreateProducer<TKey, TValue>() where TKey : class where TValue : class
        {
            return new ProducerBuilder<TKey, TValue>(_options.CurrentValue).Build();
        }

        public async Task ProduceAsync<TKey, TValue>(KafkaMessage<TKey, TValue> kafkaMessage) where TKey : class where TValue : class
        {
            using var producer = CreateProducer<TKey, TValue>();

            var message = new Message<TKey, TValue>();
            message.Headers ??= new Headers();
            message.Value = kafkaMessage.Payload;
            message.Key = kafkaMessage.Key;

            foreach (var header in kafkaMessage.Headers)
                message.Headers.Add(header.Key, Encoding.UTF8.GetBytes(header.Value));

            await producer.ProduceAsync(_options.CurrentValue.Topic, message);
        }
    }
}
