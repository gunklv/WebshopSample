namespace Catalog.IntegrationEventSenderHost.Core
{
    internal interface IIntegrationEventSenderProcess
    {
        Task RunAsync(CancellationToken cancellatonToken);
    }
}
