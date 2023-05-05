using Microsoft.Extensions.Primitives;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Models
{
    internal class KafkaConsumeResult<T>
    {
        public T Payload { get; set; }
        public IReadOnlyDictionary<string, StringValues> Headers { get; set; }
        public string Topic { get; set; }
        public long Offset { get; set; }
        public int Partition { get; set; }

        public KafkaConsumeResult(
            T payload,
            IDictionary<string, StringValues> headers,
            string topic,
            long offset,
            int partition)
        {
            Payload = payload;
            Headers = headers.ToDictionary(k => k.Key, v => v.Value);
            Topic = topic;
            Offset = offset;
            Partition = partition;
        }
    }
}
