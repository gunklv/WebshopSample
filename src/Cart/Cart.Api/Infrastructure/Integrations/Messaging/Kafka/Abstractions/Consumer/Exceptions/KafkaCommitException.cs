using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Exceptions;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Exceptions
{
    internal class KafkaCommitException : KafkaException
    {
        public KafkaCommitException(Exception innerException) : base(innerException.Message)
        {
        }
    }
}
