using Catalog.Api.Models.Category.Requests;
using FluentValidation;

namespace Catalog.Api.Validators.Category.Requests
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
