using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Exceptions;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Models;
using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Validators.Abstractions;
using FluentValidation;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.ItemPropertiesUpdatedConsumer.Validators
{
    internal class ItemPropertiesUpdatedValidator : AbstractValidator<ItemPropertiesUpdatedMessage>, IMessageValidatorService<ItemPropertiesUpdatedMessage>
    {
        public ItemPropertiesUpdatedValidator()
        {
            RuleFor(m => m.ItemId).Must(BeAValidLong);
            RuleFor(m => m.CategoryId).Must(BeAValidGuid);
            RuleFor(m => m.Price).Must(BeAValidDecimal);
            RuleFor(m => m.Amount).Must(BeAValidLong);
        }

        private bool BeAValidGuid(string arg)
        {
            return Guid.TryParse(arg, out _);
        }

        private bool BeAValidDecimal(string arg)
        {
            return decimal.TryParse(arg, out _);
        }

        private bool BeAValidLong(string arg)
        {
            return long.TryParse(arg, out _);
        }

        public Task EnsureValidityAsync(ItemPropertiesUpdatedMessage message)
        {
            var result = Validate(message);
            if (!result.IsValid)
                throw MessageValidationException.Create(message, result.Errors);

            return Task.CompletedTask;
        }
    }
}
