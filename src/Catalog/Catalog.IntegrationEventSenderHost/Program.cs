using Catalog.IntegrationEventSenderHost.Core;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Producer;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.CatalogIntegrationEventProducer;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.CatalogIntegrationEventProducer.Configuration;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Repositories;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Repositories.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Catalog.IntegrationEventSenderHost
{
    internal static class Program
    {
        public static async Task Main()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            var serviceCollection = ConfigureServices(new ServiceCollection(), CreateConfiguration());
            await using var serviceProvider = serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var integrationEventSenderProcess = scope.ServiceProvider.GetRequiredService<IIntegrationEventSenderProcess>();
            try
            {
                await integrationEventSenderProcess.RunAsync(cts.Token);
            }
            finally
            {
                Console.Write("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static IConfiguration CreateConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        private static IServiceCollection ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient((s) => new NpgsqlConnection(configuration.GetValue<string>("Persistence:ConnectionString")));

            serviceCollection.AddSingleton<IIntegrationEventSenderProcess, IntegrationEventSenderProcess>();
            serviceCollection.AddScoped<IKafkaProducer<CatalogIntegrationEventProducerConfiguration>, KafkaProducer<CatalogIntegrationEventProducerConfiguration>>();
            serviceCollection.AddScoped<ICatalogIntegrationEventMessageProducer, CatalogIntegrationEventMessageProducer>();
            serviceCollection.AddScoped<IIntegrationEventOutboxRepository, IntegrationEventOutboxRepository>();

            serviceCollection.Configure<CatalogIntegrationEventProducerConfiguration>(configuration.GetSection("CatalogIntegrationEventProducerConfiguration"));

            return serviceCollection;
        }
    }
}