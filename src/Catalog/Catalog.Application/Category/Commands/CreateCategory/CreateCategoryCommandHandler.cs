using Catalog.Application.Shared;
using Catalog.Application.Shared.Exceptions;
using MediatR;

namespace Catalog.Application.Category.Commands.CreateCategory
{
    internal class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Domain.CategoryAggregate.Category>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Domain.CategoryAggregate.Category> Handle(CreateCategoryCommand createCategoryCommand, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginAsync();
            try
            {
                Domain.CategoryAggregate.Category parentCategory = null;
                if (createCategoryCommand.ParentId != null)
                {
                    parentCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(createCategoryCommand.ParentId);
                    if (parentCategory == null)
                        throw new CategoryNotFoundException(
                            $"Could not found ParentCategory with id: {createCategoryCommand.ParentId} for Category creation.");
                }

                var category = new Domain.CategoryAggregate.Category(null, createCategoryCommand.Name, createCategoryCommand.ImageUrl, parentCategory);

                var insertedCategory = await _unitOfWork.CategoryRepository.InsertAsync(category);

                await _unitOfWork.CommitAsync();

                return insertedCategory;
            }
            catch(Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
