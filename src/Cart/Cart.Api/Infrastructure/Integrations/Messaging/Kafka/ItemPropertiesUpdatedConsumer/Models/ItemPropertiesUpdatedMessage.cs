using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Attributes;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Models;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Models
{
    [MessageType("ItemPropertiesUpdated")]
    internal class ItemPropertiesUpdatedMessage : KafkaConsumeMessage
    {
        public string ItemId { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
