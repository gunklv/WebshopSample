using Catalog.IntegrationEventSenderHost.Core.Models;

namespace Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Repositories.Abstractions
{
    internal interface IIntegrationEventOutboxRepository
    {
        Task<IntegrationEvent> UpdateIntegrationEventAsync(IntegrationEvent integrationEvent);
        Task<IReadOnlyCollection<IntegrationEvent>> GetAllNotProcessedIntegrationEventsAsync();
    }
}
