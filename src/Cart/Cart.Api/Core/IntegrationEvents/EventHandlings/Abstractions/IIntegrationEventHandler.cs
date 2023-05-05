using Cart.Api.Core.IntegrationEvents.Events.Abstractions;

namespace Cart.Api.Core.IntegrationEvents.EventHandlings.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
    {
        Task HandleAsync(TIntegrationEvent @event);
    }
}
