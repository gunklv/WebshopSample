using Confluent.Kafka;
using Newtonsoft.Json;
using System.Text;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Utils
{
    internal class KafkaMessageDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull) return default;

            var serializedMessage = Encoding.UTF8.GetString(data);

            return JsonConvert.DeserializeObject<T>(serializedMessage);
        }
    }
}
