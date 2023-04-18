using Confluent.Kafka;

namespace Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Producer
{
    internal abstract class KafkaProducerConfiguration : ProducerConfig
    {
        public string GroupId { get; set; }
        public bool EnableAutoCommit { get; set; }
        public int SessionTimeoutMs { get; set; }
        public string ResponseTopic { get; set; }
        public string KafkaCluster { get; set; }
        public string Topic { get; set; }
    }
}
