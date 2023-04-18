using Catalog.Application.Shared;
using Catalog.Application.Shared.Exceptions;
using MediatR;

namespace Catalog.Application.Category.Commands.DeleteCategory
{
    internal class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCategoryCommand deleteCategoryCommand, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginAsync();
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(deleteCategoryCommand.Id);
                if (category == null)
                    throw new CategoryNotFoundException(
                        $"Could not found Category with id: {deleteCategoryCommand.Id} for deleting Category.");

                category.MarkDeleted();

                await _unitOfWork.CategoryRepository.DeleteAsync(category);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
