using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Models;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Exceptions;
using FluentValidation.Results;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Exceptions
{
    internal class MessageValidationException : KafkaException
    {
        protected MessageValidationException(string message) : base(message)
        {
        }

        public static MessageValidationException Create<TMessage>(TMessage message, IReadOnlyList<ValidationFailure> validationFailures) where TMessage : KafkaConsumeMessage
        {
            const string messageTemplate = "Message validation failed for '{0}' with errors {1}";
            var messageType = message.GetType();

            var errors = string.Join(", ", validationFailures
                .Select(p => $"[{p.PropertyName} = '{p.AttemptedValue}': {p.ErrorMessage}]")
                .ToList());

            return new MessageValidationException(string.Format(messageTemplate, messageType.Name, errors));
        }
    }
}
