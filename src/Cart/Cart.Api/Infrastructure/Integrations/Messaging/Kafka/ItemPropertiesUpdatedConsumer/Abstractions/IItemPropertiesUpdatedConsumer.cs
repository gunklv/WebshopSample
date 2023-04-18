namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Abstractions
{
    public interface IItemPropertiesUpdatedConsumer
    {
        Task ConsumeAsync(CancellationToken cancellationToken);
    }
}
