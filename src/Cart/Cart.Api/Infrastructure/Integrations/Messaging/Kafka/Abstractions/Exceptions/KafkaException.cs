namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Exceptions
{
    internal abstract class KafkaException : Exception
    {
        protected KafkaException(string message) : base(message)
        {
        }
    }
}
