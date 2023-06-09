﻿using Catalog.IntegrationEventSenderHost.Core;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Producer;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.CatalogIntegrationEventProducer;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Messaging.Kafka.CatalogIntegrationEventProducer.Configuration;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Persistance.PostgreSql.Repositories;
using Catalog.IntegrationEventSenderHost.Infrastructure.Integrations.Persistance.PostgreSql.Repositories.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                Console.Read();
            }
        }

        private static IConfiguration CreateConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "prod")
            {
                builder.AddJsonFile("appsettings.prod.json");
            }

            return builder.Build();
        }

        private static IServiceCollection ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddLogging(configure => configure.AddConsole());

            serviceCollection.AddTransient((s) => new NpgsqlConnection(configuration.GetValue<string>("PostgreSqlConfiguration:ConnectionString")));

            serviceCollection.AddSingleton<IIntegrationEventSenderProcess, IntegrationEventSenderProcess>();
            serviceCollection.AddScoped<IKafkaProducer<CatalogIntegrationEventProducerConfiguration>, KafkaProducer<CatalogIntegrationEventProducerConfiguration>>();
            serviceCollection.AddScoped<ICatalogIntegrationEventMessageProducer, CatalogIntegrationEventMessageProducer>();
            serviceCollection.AddScoped<IIntegrationEventOutboxRepository, IntegrationEventOutboxRepository>();

            serviceCollection.Configure<CatalogIntegrationEventProducerConfiguration>(configuration.GetSection("CatalogIntegrationEventProducerConfiguration"));

            return serviceCollection;
        }
    }
}