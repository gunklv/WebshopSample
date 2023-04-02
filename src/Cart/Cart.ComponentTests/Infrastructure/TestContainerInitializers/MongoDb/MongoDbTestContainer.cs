using Cart.Api.Infrastructure.Configurations;
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

        public PersistenceConfiguration GetConfiguration()
            => new PersistenceConfiguration
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
