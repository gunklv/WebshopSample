using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Configuration;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Models;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer
{
    internal interface IKafkaConsumer<TConfig, TMessage>
        where TConfig : KafkaConsumerConfiguration
        where TMessage : KafkaConsumeMessage
    {
        Task<KafkaConsumeResult<TMessage>> ConsumeAsync(CancellationToken cancellationToken);
        void Commit(KafkaConsumeResult<TMessage> result);
        void Close();
    }
}
