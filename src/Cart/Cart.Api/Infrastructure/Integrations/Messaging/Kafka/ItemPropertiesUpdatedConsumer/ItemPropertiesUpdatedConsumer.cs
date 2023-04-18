using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Exceptions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Models;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer;
using Confluent.Kafka;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Models;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Validators.Abstractions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Abstractions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Configurations;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Mappers.Abstractions;
using Cart.Api.Core.IntegrationEvents.Events;
using Cart.Api.Core.IntegrationEvents.EventHandlings.Abstractions;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer
{
    internal class ItemPropertiesUpdatedConsumer : IItemPropertiesUpdatedConsumer
    {
        private readonly IKafkaConsumer<ItemPropertiesUpdatedConsumerConfiguration, ItemPropertiesUpdatedMessage> _kafkaConsumer;
        private readonly IMessageValidatorService<ItemPropertiesUpdatedMessage> _messageValidatorService;
        private readonly IIntegrationIEventMapper<ItemPropertiesUpdatedMessage, ItemPropertiesUpdatedIntegrationEvent> _mapper;
        private readonly IIntegrationEventHandler<ItemPropertiesUpdatedIntegrationEvent> _itemPropertiesUpdatedIntegrationEventHandler;
        private readonly ILogger<ItemPropertiesUpdatedConsumer> _logger;

        public ItemPropertiesUpdatedConsumer(
            IKafkaConsumer<ItemPropertiesUpdatedConsumerConfiguration, ItemPropertiesUpdatedMessage> kafkaConsumer,
            IMessageValidatorService<ItemPropertiesUpdatedMessage> messageValidatorService,
            IIntegrationIEventMapper<ItemPropertiesUpdatedMessage, ItemPropertiesUpdatedIntegrationEvent> mapper,
            IIntegrationEventHandler<ItemPropertiesUpdatedIntegrationEvent> itemPropertiesUpdatedIntegrationEventHandler,
            ILogger<ItemPropertiesUpdatedConsumer> logger)
        {
            _kafkaConsumer = kafkaConsumer;
            _messageValidatorService = messageValidatorService;
            _mapper = mapper;
            _itemPropertiesUpdatedIntegrationEventHandler = itemPropertiesUpdatedIntegrationEventHandler;
            _logger = logger;
        }

        public async Task ConsumeAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ItemPropertiesUpdatedConsumer)} starting up on thread '{Thread.CurrentThread.ManagedThreadId}'");

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await InternalConsumeAsync(cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Operation Cancelled - Consumer stopping");
            }
            finally
            {
                _kafkaConsumer.Close();
                _logger.LogInformation("Consumer closed");
            }
        }

        protected async Task InternalConsumeAsync(CancellationToken cancellationToken)
        {
            KafkaConsumeResult<ItemPropertiesUpdatedMessage> consumeResult = null;
            try
            {
                consumeResult = await _kafkaConsumer.ConsumeAsync(cancellationToken);

                if (consumeResult == null)
                    return;
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex, "Consumer did not manage to consume the message.");
                throw;
            }

            using (_logger.BeginScope(CreateLoggerState(consumeResult)))
            {
                try
                {
                    _logger.LogInformation("Processing incoming message with payload: '{@Payload}'", consumeResult.Payload);

                    await _messageValidatorService.EnsureValidityAsync(consumeResult.Payload);

                    var event_ = _mapper.Map(consumeResult.Payload);

                    await _itemPropertiesUpdatedIntegrationEventHandler.HandleAsync(event_);

                    _kafkaConsumer.Commit(consumeResult);
                }
                catch (KafkaCommitException ex)
                {
                    _logger.LogError(ex, "Unexpected error during offset commit: Partition - '{@Partition}' Offset - '{@Offset}'", consumeResult.Partition, consumeResult.Offset);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{nameof(ItemPropertiesUpdatedConsumer)} failed to process message");
                }
            }
        }
        private Dictionary<string, object> CreateLoggerState(KafkaConsumeResult<ItemPropertiesUpdatedMessage> kafkaConsumeResult) =>
            new()
            {
                { "ItemId", kafkaConsumeResult.Payload.ItemId }
            };
    }
}
