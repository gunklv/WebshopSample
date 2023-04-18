using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.CatalogIntegrationEventProducer;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Persistance.PostgreSql.Repositories.Abstractions;

namespace Catalog.IntegrationEventSenderHost.Core
{
    internal class IntegrationEventSenderProcess : IIntegrationEventSenderProcess
    {
        private readonly TimeSpan ITERATION_FREQUENCY = TimeSpan.FromSeconds(3);

        private readonly IIntegrationEventOutboxRepository _integrationEventOutboxRepository;
        private readonly ICatalogIntegrationEventMessageProducer _catalogIntegrationEventMessageProducer;

        public IntegrationEventSenderProcess(
            IIntegrationEventOutboxRepository integrationEventOutboxRepository,
            ICatalogIntegrationEventMessageProducer catalogIntegrationEventMessageProducer)
        {
            _integrationEventOutboxRepository = integrationEventOutboxRepository;
            _catalogIntegrationEventMessageProducer = catalogIntegrationEventMessageProducer;
        }

        public async Task RunAsync(CancellationToken cancellatonToken)
        {
            while (!cancellatonToken.IsCancellationRequested)
            {
                var nonProcessedIntegrationEvents = await _integrationEventOutboxRepository.GetAllNotProcessedIntegrationEventsAsync();

                foreach (var integrationEvent in nonProcessedIntegrationEvents)
                {
                    await _catalogIntegrationEventMessageProducer.PublishEvent(integrationEvent);

                    Console.WriteLine($"Event with id [{integrationEvent.EventId}] and type [{integrationEvent.EventType}] has been published.");

                    integrationEvent.ProcessedOn = DateTime.UtcNow;
                    await _integrationEventOutboxRepository.UpdateIntegrationEventAsync(integrationEvent);
                }

                await Task.Delay(ITERATION_FREQUENCY);
            }
        }
    }
}
