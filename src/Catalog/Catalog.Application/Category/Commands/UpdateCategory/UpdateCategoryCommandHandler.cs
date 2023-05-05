using Catalog.Application.Shared;
using Catalog.Application.Shared.Exceptions;
using MediatR;

namespace Catalog.Application.Category.Commands.UpdateCategory
{
    internal class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Domain.CategoryAggregate.Category>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Domain.CategoryAggregate.Category> Handle(UpdateCategoryCommand updateCategoryCommand, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginAsync();
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(updateCategoryCommand.Id);

                if (category == null)
                    throw new CategoryNotFoundException($"Could not found Category with id: {updateCategoryCommand.Id} for updating Category.");

                Domain.CategoryAggregate.Category parentCategory = null;
                if (updateCategoryCommand.ParentId != null)
                {
                    parentCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(updateCategoryCommand.ParentId);

                    if (parentCategory == null)
                        throw new CategoryNotFoundException(
                            $"Could not found ParentCategory with id: {updateCategoryCommand.ParentId} for updating Category with id: {updateCategoryCommand.Id}.");
                }

                category.UpdateProperties(parentCategory, updateCategoryCommand.Name, updateCategoryCommand.ImageUrl);

                var updatedCategory = await _unitOfWork.CategoryRepository.UpdateAsync(category);

                await _unitOfWork.CommitAsync();

                return updatedCategory;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
