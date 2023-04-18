namespace Catalog.Infrastructure.Persistance.Sql.Models
{
    internal class IntegrationEventDto
    {
        public Guid EventId { get; set; }
        public string EventType { get; set; }
        public string AggregateId { get; set; }
        public string AggregateType { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
    }
}
