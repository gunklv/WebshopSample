﻿using Catalog.Api.Models.Item.Requests;
using FluentValidation;

namespace Catalog.Api.Validators.Category.Requests
{
    public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
    {
        public CreateItemRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(request => request.CategoryId)
                .NotEmpty();

            RuleFor(request => request.Price)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);

            RuleFor(request => request.Amount)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);
        }
    }
}
