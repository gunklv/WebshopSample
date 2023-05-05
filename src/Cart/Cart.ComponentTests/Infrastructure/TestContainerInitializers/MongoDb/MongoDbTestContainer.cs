using Cart.Api.Infrastructure.Integrations.Persistance.MongoDb.Repositories.Configurations;
using Testcontainers.MongoDb;

namespace Cart.ComponentTests.Infrastructure.TestContainerInitializers.MongoDb
{
    public class MongoDbTestContainer
    {
        private const string DatabaseName = "WebshopSample";

        private readonly MongoDbContainer _container;


        public MongoDbTestContainer(MongoDbContainer container)
        {
            _container = container;
        }

        public async Task StartAsync()
        {
            await _container.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _container.DisposeAsync();
        }

        public Api.Infrastructure.Integrations.Persistance.MongoDb.Repositories.Configurations.MongoDbConfiguration GetConfiguration()
            => new Api.Infrastructure.Integrations.Persistance.MongoDb.Repositories.Configurations.MongoDbConfiguration
            {
                ConnectionString = _container.GetConnectionString(),
                DatabaseName = DatabaseName
            };

        public async Task GenerateDatabaseSchemaAsync()
        {
            await _container.ExecScriptAsync(
                @$"db = db.getSiblingDB('{DatabaseName}');
                  db.createCollection('Cart');"
            );
        }
    }
}
