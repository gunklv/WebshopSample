using Cart.Api.Core.Abstractions.Repositories;
using Cart.Api.Core.IntegrationEvents.Events;
using Cart.Api.Infrastructure.Configurations;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Abstractions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Configurations;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Mappers;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Mappers.Abstractions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Models;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Validators;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Validators.Abstractions;
using Cart.Api.Infrastructure.Repositories;

namespace Cart.Api.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<PersistenceConfiguration>(configuration.GetSection("Persistence"));
            serviceCollection.Configure<ItemPropertiesUpdatedConsumerConfiguration>(configuration.GetSection("ItemPropertiesUpdatedConsumer"));

            serviceCollection.AddSingleton<ICartRepository, CartRepository>();

            serviceCollection.AddSingleton<
                IKafkaConsumer<ItemPropertiesUpdatedConsumerConfiguration, ItemPropertiesUpdatedMessage>,
                KafkaConsumer<ItemPropertiesUpdatedConsumerConfiguration, ItemPropertiesUpdatedMessage>>();

            serviceCollection.AddSingleton<IItemPropertiesUpdatedConsumer, ItemPropertiesUpdatedConsumer>();

            serviceCollection.AddSingleton<IIntegrationIEventMapper<ItemPropertiesUpdatedMessage, ItemPropertiesUpdatedIntegrationEvent>, ItemPropertiesUpdatedEventMapper>();
            serviceCollection.AddSingleton<IMessageValidatorService<ItemPropertiesUpdatedMessage>, ItemPropertiesUpdatedValidator>();
        }
    }
}
