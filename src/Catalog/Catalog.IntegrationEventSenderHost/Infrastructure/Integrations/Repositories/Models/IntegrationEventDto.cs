namespace Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Repositories.Models
{
    internal class IntegrationEventDto
    {
        public Guid EventId { get; set; }
        public string EventType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public string Payload { get; set; }
    }
}
