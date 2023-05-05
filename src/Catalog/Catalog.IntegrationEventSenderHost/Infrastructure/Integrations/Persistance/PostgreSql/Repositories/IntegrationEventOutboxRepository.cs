using Catalog.IntegrationEventSenderHost.Core.Models;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Persistance.PostgreSql.Repositories.Abstractions;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Persistance.PostgreSql.Repositories.Models;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Data;

namespace Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Persistance.PostgreSql.Repositories
{
    internal class IntegrationEventOutboxRepository : IIntegrationEventOutboxRepository
    {
        private const string TABLE_NAME = "integrationEventOutbox";

        private readonly NpgsqlConnection _npgsqlConnection;

        public IntegrationEventOutboxRepository(NpgsqlConnection npgsqlConnection)
        {
            _npgsqlConnection = npgsqlConnection;
        }

        public async Task<IntegrationEvent> UpdateIntegrationEventAsync(IntegrationEvent integrationEvent)
        {
            await _npgsqlConnection.OpenAsync();

            var commandText = $@"UPDATE {TABLE_NAME}
                SET eventType = @eventType, createdOn = @createdOn, processedOn = @processedOn, payload = @payload
                WHERE eventId = @eventId";

            var queryArguments = new IntegrationEventDto
            {
                EventId = integrationEvent.EventId,
                EventType = integrationEvent.EventType,
                CreatedOn = integrationEvent.CreatedOn,
                ProcessedOn = integrationEvent.ProcessedOn,
                Payload = JsonConvert.SerializeObject(integrationEvent.Payload)
            };

            await _npgsqlConnection.ExecuteAsync(commandText, queryArguments);

            await _npgsqlConnection.CloseAsync();

            return new IntegrationEvent();
        }

        public async Task<IReadOnlyCollection<IntegrationEvent>> GetAllNotProcessedIntegrationEventsAsync()
        {
            await _npgsqlConnection.OpenAsync();

            string commandText = $"SELECT * FROM {TABLE_NAME} WHERE ProcessedOn IS NULL";

            var integrationEventDtoEnumerable = await _npgsqlConnection.QueryAsync<IntegrationEventDto>(commandText);

            await _npgsqlConnection.CloseAsync();

            return integrationEventDtoEnumerable.Select(x => new IntegrationEvent
            {
                EventId = x.EventId,
                EventType = x.EventType,
                CreatedOn = x.CreatedOn,
                ProcessedOn = x.ProcessedOn,
                Payload = JsonConvert.DeserializeObject(x.Payload)
            }).ToList();
        }
    }
}
