using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Attributes;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Configuration;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Exceptions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Extensions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Models;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Utils;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Reflection;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer
{
    internal class KafkaConsumer<TConfig, TMessage> : IKafkaConsumer<TConfig, TMessage>, IDisposable
        where TConfig : KafkaConsumerConfiguration
        where TMessage : KafkaConsumeMessage
    {
        private readonly Stopwatch _lastCommitWatch = Stopwatch.StartNew();
        private readonly TimeSpan _autoCommitElapsedTimeThreshold = TimeSpan.FromMinutes(1);
        private readonly string _messageType = GetMessageType(typeof(TMessage));

        private IConsumer<Ignore, TMessage> _consumer;
        private readonly IOptionsMonitor<TConfig> _optionsMonitor;
        private readonly ILogger<KafkaConsumer<TConfig, TMessage>> _logger;

        public KafkaConsumer(
            IOptionsMonitor<TConfig> optionsMonitor,
            ILogger<KafkaConsumer<TConfig, TMessage>> logger)
        {
            _consumer = CreateConsumer(optionsMonitor.CurrentValue);
            _optionsMonitor = optionsMonitor;
            _logger = logger;

            optionsMonitor.OnChange(OnOptionsChangeListener);
        }

        public async Task<KafkaConsumeResult<TMessage>> ConsumeAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            var consumeResult = _consumer.Consume(cancellationToken);

            if (!IsMessageTypeValid(consumeResult))
            {
                if (_lastCommitWatch.Elapsed > _autoCommitElapsedTimeThreshold)
                    Commit(consumeResult);

                return null;
            }

            return new KafkaConsumeResult<TMessage>(
                consumeResult.Message.Value, consumeResult.Message.Headers.ExtractHeaders(), consumeResult.Topic, consumeResult.Offset, consumeResult.Partition);
        }

        public void Commit(KafkaConsumeResult<TMessage> result)
            => Commit(new TopicPartitionOffset(result.Topic, new Partition(result.Partition), new Offset(result.Offset)));

        public void Commit(ConsumeResult<Ignore, TMessage> result)
            => Commit(new TopicPartitionOffset(result.Topic, new Partition(result.Partition), new Offset(result.Offset)));

        private void Commit(TopicPartitionOffset topicPartitionOffset)
        {
            try
            {
                _consumer.Commit(new[] { topicPartitionOffset });
                _lastCommitWatch.Restart();
            }
            catch (Exception ex)
            {
                throw new KafkaCommitException(ex);
            }
        }

        public void Close() => _consumer.Close();

        public void Dispose()
        {
            _consumer?.Dispose();
        }

        private void OnOptionsChangeListener(TConfig config)
        {
            _consumer = CreateConsumer(config);
        }

        private IConsumer<Ignore, TMessage> CreateConsumer(TConfig config)
        {
            if (_consumer != null)
            {
                try
                {
                    _consumer.Close();
                    _consumer.Dispose();
                }
                catch (Exception exception)
                {
                    _logger.LogWarning(exception, exception.Message);
                }
            }

            var consumer = new ConsumerBuilder<Ignore, TMessage>(config)
                .SetValueDeserializer(new KafkaMessageDeserializer<TMessage>())
                .SetErrorHandler((_, error) =>
                {
                    _logger.Log(error.IsFatal ? LogLevel.Error : LogLevel.Warning, error.Reason);
                })
                .Build();

            consumer.Subscribe(config.Topic);

            return consumer;
        }

        private static string GetMessageType(Type genericType)
        {
            var messageTypeAttribute = genericType.GetCustomAttribute<MessageTypeAttribute>(false);
            return messageTypeAttribute != null ? messageTypeAttribute.MessageType : nameof(TMessage);
        }

        private bool IsMessageTypeValid(ConsumeResult<Ignore, TMessage> consumeResult)
            => consumeResult.Message.Headers.FirstOrDefault(header => header.Key == "EventType").GetValue() == _messageType;
    }
}
