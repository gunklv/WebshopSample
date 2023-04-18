namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Producer.Models
{
    public class KafkaProduceMessage<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        public Dictionary<string, string> Headers { get; set; }
        public TValue Payload { get; set; }
        public TKey Key { get; set; }
    }
}
