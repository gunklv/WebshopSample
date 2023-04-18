namespace Catalog.IntegrationEventSenderHost.Core.Models
{
    public class IntegrationEvent
    {
        public Guid EventId { get; set; }
        public string EventType { get; set; }
        public string AggregateId { get; set; }
        public string AggregateType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public object Payload { get; set; }
    }
}
