using Cart.Api.Core.Abstractions.Repositories;
using Cart.Api.Core.IntegrationEvents.Events;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Abstractions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Configurations;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Mappers;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Mappers.Abstractions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Models;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Validators;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Validators.Abstractions;
using Cart.Api.Infrastructure.Integrations.Persistance.MongoDb.Repositories;
using Cart.Api.Infrastructure.Integrations.Persistance.MongoDb.Repositories.Configurations;

namespace Cart.Api.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<MongoDbConfiguration>(configuration.GetSection("MongoDbConfiguration"));

            serviceCollection.AddSingleton<ICartRepository, CartRepository>();

            serviceCollection.Configure<
                IntegrationEventKafkaConsumerConfiguration<ItemPropertiesUpdatedMessage>>(
                configuration.GetSection("ItemPropertiesUpdatedConsumerConfiguration"));

            serviceCollection.AddSingleton(typeof(IKafkaConsumer<,>), typeof(KafkaConsumer<,>));

            serviceCollection.AddSingleton<
                IIntegrationEventConsumer,
                IntegrationEventConsumer<ItemPropertiesUpdatedMessage, ItemPropertiesUpdatedIntegrationEvent>>();

            serviceCollection.AddSingleton<
                IIntegrationIEventMapper<ItemPropertiesUpdatedMessage, ItemPropertiesUpdatedIntegrationEvent>,
                ItemPropertiesUpdatedEventMapper>();

            serviceCollection.AddSingleton<
                IMessageValidatorService<ItemPropertiesUpdatedMessage>,
                ItemPropertiesUpdatedValidator>();
        }
    }
}
