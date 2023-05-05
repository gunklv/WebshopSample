using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Configuration;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Models;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Configurations
{
    internal class IntegrationEventKafkaConsumerConfiguration<T> : KafkaConsumerConfiguration where T: IntegrationEventConsumeMessage
    {
    }
}
