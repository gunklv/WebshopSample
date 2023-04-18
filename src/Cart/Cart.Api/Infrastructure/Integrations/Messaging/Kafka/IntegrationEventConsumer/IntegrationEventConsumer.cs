using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Exceptions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Models;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer;
using Confluent.Kafka;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Validators.Abstractions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Configurations;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Mappers.Abstractions;
using Cart.Api.Core.IntegrationEvents.EventHandlings.Abstractions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Models;
using Cart.Api.Core.IntegrationEvents.Events.Abstractions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Abstractions;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer
{
    internal class IntegrationEventConsumer<TMessage, TEvent> : IIntegrationEventConsumer<TMessage, TEvent>
        where TMessage : IntegrationEventConsumeMessage
        where TEvent : IntegrationEvent
    {
        private readonly IKafkaConsumer<IntegrationEventKafkaConsumerConfiguration<TMessage>, TMessage> _kafkaConsumer;
        private readonly IMessageValidatorService<TMessage> _messageValidatorService;
        private readonly IIntegrationIEventMapper<TMessage, TEvent> _mapper;
        private readonly IIntegrationEventHandler<TEvent> _integrationEventHandler;
        private readonly ILogger<IntegrationEventConsumer<TMessage, TEvent>> _logger;

        public IntegrationEventConsumer(
            IKafkaConsumer<IntegrationEventKafkaConsumerConfiguration<TMessage>, TMessage> kafkaConsumer,
            IMessageValidatorService<TMessage> messageValidatorService,
            IIntegrationIEventMapper<TMessage, TEvent> mapper,
            IIntegrationEventHandler<TEvent> integrationEventHandler,
            ILogger<IntegrationEventConsumer<TMessage, TEvent>> logger)
        {
            _kafkaConsumer = kafkaConsumer;
            _messageValidatorService = messageValidatorService;
            _mapper = mapper;
            _integrationEventHandler = integrationEventHandler;
            _logger = logger;
        }

        public async Task ConsumeAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(IntegrationEventConsumer)} starting up on thread '{Thread.CurrentThread.ManagedThreadId}'");

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
            KafkaConsumeResult<TMessage> consumeResult = null;
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

                    var @event = _mapper.Map(consumeResult.Payload);

                    await _integrationEventHandler.HandleAsync(@event);

                    _kafkaConsumer.Commit(consumeResult);
                }
                catch (KafkaCommitException ex)
                {
                    _logger.LogError(ex, "Unexpected error during offset commit: Partition - '{@Partition}' Offset - '{@Offset}'", consumeResult.Partition, consumeResult.Offset);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{nameof(IntegrationEventConsumer)} failed to process message");
                }
            }
        }
        private Dictionary<string, object> CreateLoggerState(KafkaConsumeResult<TMessage> kafkaConsumeResult) =>
            new()
            {
                { "EventType", kafkaConsumeResult.Headers["EventType"] }
            };
    }
}
