using Confluent.Kafka;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Extensions
{
    internal static class KafkaExtensions
    {
        public static string GetValue(this IHeader header)
            => Encoding.UTF8.GetString(header.GetValueBytes());

        public static IDictionary<string, StringValues> ExtractHeaders(this Headers messageHeaders)
        {
            var headers = new Dictionary<string, StringValues>();
            foreach (var headerCollection in messageHeaders.GroupBy(x => x.Key))
            {
                var values = headerCollection.Select(x => x.GetValue());
                headers[headerCollection.Key] = new StringValues(values.ToArray());
            }
            return headers;
        }
    }
}
