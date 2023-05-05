using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Abstractions;

namespace Cart.Api.Api.Services
{
    public class IntegrationEventProcessorService : BackgroundService
    {
        private readonly IEnumerable<IIntegrationEventConsumer> _integrationEventConsumers;
        private readonly ILogger<IntegrationEventProcessorService> _logger;

        public IntegrationEventProcessorService(
            IEnumerable<IIntegrationEventConsumer> integrationEventConsumers,
            ILogger<IntegrationEventProcessorService> logger)
        {
            _integrationEventConsumers = integrationEventConsumers;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            _logger.LogInformation("Background service execution started.");

            foreach (var consumer in _integrationEventConsumers)
            {
                await consumer.ConsumeAsync(cancellationToken);
            }
        }
    }
}
