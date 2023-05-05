using Catalog.Application.Shared.Models.Abstractions;

namespace Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Abstractions
{
    internal interface IIntegrationEventOutboxRepository
    {
        Task InsertAsync(IntegrationEvent integrationEvent);
    }
}
