using Catalog.Application.Shared.Models.Abstractions;

namespace Catalog.Infrastructure.Persistance.Sql.Repositories.Abstractions
{
    internal interface IIntegrationEventOutboxRepository
    {
        Task InsertAsync(IntegrationEvent integrationEvent);
    }
}
