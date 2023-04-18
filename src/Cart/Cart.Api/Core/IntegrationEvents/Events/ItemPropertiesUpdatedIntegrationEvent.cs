using Cart.Api.Core.IntegrationEvents.Events.Abstractions;

namespace Cart.Api.Core.IntegrationEvents.Events
{
    public class ItemPropertiesUpdatedIntegrationEvent : IntegrationEvent
    {
        public long ItemId { get; private set; }
        public Guid CategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public long Amount { get; private set; }
        public decimal Price { get; private set; }
        public string ImageUrl { get; private set; }

        public ItemPropertiesUpdatedIntegrationEvent(
            long itemId, Guid categoryId, string name, string description, long amount, decimal price, string imageUrl)
        {
            ItemId = itemId;
            CategoryId = categoryId;
            Name = name;
            Description = description;
            Amount = amount;
            Price = price;
            ImageUrl = imageUrl;
        }
    }
}
