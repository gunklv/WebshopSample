using Confluent.Kafka;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Configuration
{
    internal class KafkaConsumerConfiguration : ConsumerConfig
    {
        public KafkaConsumerConfiguration()
        {
            base.EnableAutoCommit = false;
        }

        public string Topic { get; set; }
        public new bool EnableAutoCommit { get; } = false;
    }
}
