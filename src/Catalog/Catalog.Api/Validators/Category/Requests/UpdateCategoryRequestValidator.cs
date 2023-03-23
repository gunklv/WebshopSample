using Catalog.Api.Models.Category.Requests;
using FluentValidation;

namespace Catalog.Api.Validators.Category.Requests
{
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
