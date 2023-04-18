using Newtonsoft.Json;

namespace Catalog.Application.Shared.Models.Abstractions
{
    public abstract class IntegrationEvent
    {
        public IntegrationEvent()
        {
            CreatedOn = DateTime.UtcNow;
        }

        [JsonIgnore]
        public abstract string EventType { get; }

        [JsonIgnore]
        public DateTime CreatedOn { get; private init; } = DateTime.UtcNow;
    }
}
