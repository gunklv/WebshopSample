using Catalog.Application.Shared.Exceptions;
using Catalog.Domain.Abstractions;
using MediatR;

namespace Catalog.Application.Category.Commands.UpdateCategory
{
    internal class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Domain.Aggregates.Category>
    {
        private readonly IRepository<Domain.Aggregates.Category> _categoryRepository;

        public UpdateCategoryCommandHandler(IRepository<Domain.Aggregates.Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Domain.Aggregates.Category> Handle(UpdateCategoryCommand updateCategoryCommand, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(updateCategoryCommand.Id);

            if(category == null)
                throw new CategoryNotFoundException($"Could not found Category with id: {updateCategoryCommand.Id} for updating Category.");

            Domain.Aggregates.Category parentCategory = null;
            if(updateCategoryCommand.ParentId != null)
            {
                parentCategory = await _categoryRepository.GetByIdAsync(updateCategoryCommand.ParentId);

                if(parentCategory == null)
                    throw new CategoryNotFoundException(
                        $"Could not found ParentCategory with id: {updateCategoryCommand.ParentId} for updating Category with id: {updateCategoryCommand.Id}.");
            }

            category.Name= updateCategoryCommand.Name;
            category.ImageUrl = updateCategoryCommand.ImageUrl;
            category.ParentCategory = parentCategory;

            return await _categoryRepository.UpdateAsync(category);
        }
    }
}
