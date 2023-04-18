using Catalog.Application.Shared.Exceptions;
using Catalog.Domain.Abstractions;
using MediatR;

namespace Catalog.Application.Category.Commands.CreateCategory
{
    internal class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Domain.Aggregates.Category>
    {
        private readonly IRepository<Domain.Aggregates.Category> _categoryRepository;

        public CreateCategoryCommandHandler(IRepository<Domain.Aggregates.Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Domain.Aggregates.Category> Handle(CreateCategoryCommand createCategoryCommand, CancellationToken cancellationToken)
        {
            Domain.Aggregates.Category parentCategory = null;
            if (createCategoryCommand.ParentId != null)
            {
                parentCategory = await _categoryRepository.GetByIdAsync(createCategoryCommand.ParentId);
                if (parentCategory == null)
                    throw new CategoryNotFoundException(
                        $"Could not found ParentCategory with id: {createCategoryCommand.ParentId} for Category creation.");
            }

            var category = new Domain.Aggregates.Category(null, createCategoryCommand.Name, createCategoryCommand.ImageUrl, parentCategory);

            var insertedCategory = await _categoryRepository.InsertAsync(category);

            return insertedCategory;
        }
    }
}
