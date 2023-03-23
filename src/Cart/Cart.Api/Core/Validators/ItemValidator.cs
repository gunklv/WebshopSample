using FluentValidation;
using Cart.Api.Core.Models;
using Cart.Api.Core.Validators.Abstractions;
using Cart.Api.Core.Exceptions;

namespace Cart.Api.Core.Validators
{
    public class ItemValidator : AbstractValidator<Item>, IDomainValidator<Item>
    {
        public ItemValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
        }

        public Task EnsureValidityAsync(Item item)
        {
            var result = Validate(item);
            if (result.IsValid)
                return Task.CompletedTask;

            throw new InvalidStateException();
        }
    }
}
