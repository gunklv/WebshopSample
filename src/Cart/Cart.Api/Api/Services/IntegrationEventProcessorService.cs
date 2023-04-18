using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Abstractions;

namespace Cart.Api.Api.Services
{
    public class IntegrationEventProcessorService : BackgroundService
    {
        private readonly IItemPropertiesUpdatedConsumer _itemPropertiesUpdatedConsumer;
        private readonly ILogger<IntegrationEventProcessorService> _logger;

        public IntegrationEventProcessorService(
            IItemPropertiesUpdatedConsumer itemPropertiesUpdatedConsumer,
            ILogger<IntegrationEventProcessorService> logger)
        {
            _itemPropertiesUpdatedConsumer = itemPropertiesUpdatedConsumer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            _logger.LogInformation("Background service execution started.");

            await _itemPropertiesUpdatedConsumer.ConsumeAsync(cancellationToken);
        }
    }
}
