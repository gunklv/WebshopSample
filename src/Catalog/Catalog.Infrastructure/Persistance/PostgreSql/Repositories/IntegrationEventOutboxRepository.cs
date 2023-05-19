using Catalog.Application.Shared.Models.Abstractions;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Abstractions;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Models;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Data;

namespace Catalog.Infrastructure.Persistance.PostgreSql.Repositories
{
    internal class IntegrationEventOutboxRepository : IIntegrationEventOutboxRepository
    {
        private const string TABLE_NAME = "integrationEventOutbox";

        private readonly NpgsqlConnection _npgsqlConnection;
        private readonly IDbTransaction _dbTransaction;

        public IntegrationEventOutboxRepository(NpgsqlConnection npgsqlConnection, IDbTransaction dbTransaction)
        {
            _npgsqlConnection = npgsqlConnection;
            _dbTransaction = dbTransaction;
        }

        public async Task InsertAsync(IntegrationEvent integrationEvent)
        {
            var sql = $"INSERT INTO {TABLE_NAME} (eventType, payload, createdOn) " +
                $"VALUES (@eventType, @payload, @createdOn)";

            var queryArguments = new IntegrationEventDto
            {
                EventType = integrationEvent.EventType,
                Payload = JsonConvert.SerializeObject(integrationEvent),
                CreatedOn = integrationEvent.CreatedOn
            };

            await _npgsqlConnection.ExecuteScalarAsync<Guid>(sql, queryArguments, _dbTransaction);
        }
    }
}
