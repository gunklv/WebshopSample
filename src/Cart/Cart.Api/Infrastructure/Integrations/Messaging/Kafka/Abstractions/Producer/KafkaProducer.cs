using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Producer.Configuration;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Producer.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Producer
{
    internal class KafkaProducer<TConfig> : IKafkaProducer<TConfig> where TConfig : KafkaProducerConfiguration
    {
        private readonly ILogger<KafkaProducer<TConfig>> _logger;
        private readonly IOptionsMonitor<TConfig> _options;

        public KafkaProducer(ILogger<KafkaProducer<TConfig>> logger, IOptionsMonitor<TConfig> options)
        {
            _logger = logger;
            _options = options;
        }

        private IProducer<TKey, TValue> CreateProducer<TKey, TValue>() where TKey : class where TValue : class
        {
            return new ProducerBuilder<TKey, TValue>(_options.CurrentValue)
                .SetErrorHandler((_, error) => { _logger.Log(error.IsFatal ? LogLevel.Error : LogLevel.Warning, error.Reason); })
                .Build();
        }

        public async Task ProduceAsync<TKey, TValue>(KafkaProduceMessage<TKey, TValue> kafkaMessage) where TKey : class where TValue : class
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
