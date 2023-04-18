using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.CatalogIntegrationEventProducer;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Persistance.PostgreSql.Repositories.Abstractions;
using Microsoft.Extensions.Logging;

namespace Catalog.IntegrationEventSenderHost.Core
{
    internal class IntegrationEventSenderProcess : IIntegrationEventSenderProcess
    {
        private readonly TimeSpan ITERATION_FREQUENCY = TimeSpan.FromSeconds(3);

        private readonly IIntegrationEventOutboxRepository _integrationEventOutboxRepository;
        private readonly ICatalogIntegrationEventMessageProducer _catalogIntegrationEventMessageProducer;
        private readonly ILogger<IntegrationEventSenderProcess> _logger;

        public IntegrationEventSenderProcess(
            IIntegrationEventOutboxRepository integrationEventOutboxRepository,
            ICatalogIntegrationEventMessageProducer catalogIntegrationEventMessageProducer,
            ILogger<IntegrationEventSenderProcess> logger)
        {
            _integrationEventOutboxRepository = integrationEventOutboxRepository;
            _catalogIntegrationEventMessageProducer = catalogIntegrationEventMessageProducer;
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken cancellatonToken)
        {
            while (!cancellatonToken.IsCancellationRequested)
            {
                var nonProcessedIntegrationEvents = await _integrationEventOutboxRepository.GetAllNotProcessedIntegrationEventsAsync();

                foreach (var integrationEvent in nonProcessedIntegrationEvents)
                {
                    await _catalogIntegrationEventMessageProducer.PublishEvent(integrationEvent);

                    _logger.LogInformation($"Event with id [{integrationEvent.EventId}] and type [{integrationEvent.EventType}] has been published.");

                    integrationEvent.ProcessedOn = DateTime.UtcNow;
                    await _integrationEventOutboxRepository.UpdateIntegrationEventAsync(integrationEvent);
                }

                await Task.Delay(ITERATION_FREQUENCY);
            }
        }
    }
}
