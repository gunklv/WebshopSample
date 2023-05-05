using Catalog.Application.Shared.Models.Abstractions;
using Catalog.Domain.Shared.Events;

namespace Catalog.Application.Shared.Mappers.Abstractions
{
    public interface IIntegrationEventMapper
    {
        IntegrationEvent MapDomainEvent(DomainEvent domainEvent);
    }
}
