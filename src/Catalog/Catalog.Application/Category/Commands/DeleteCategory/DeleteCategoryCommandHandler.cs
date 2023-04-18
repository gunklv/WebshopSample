using Catalog.Application.Shared.Exceptions;
using Catalog.Domain.Abstractions;
using MediatR;

namespace Catalog.Application.Category.Commands.DeleteCategory
{
    internal class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IRepository<Domain.Aggregates.Category> _categoryRepository;
        private readonly IItemRepository _itemRepository;

        public DeleteCategoryCommandHandler(
            IRepository<Domain.Aggregates.Category> categoryRepository,
            IItemRepository itemRepository)
        {
            _categoryRepository = categoryRepository;
            _itemRepository = itemRepository;
        }

        public async Task Handle(DeleteCategoryCommand deleteCategoryCommand, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(deleteCategoryCommand.Id);
            if (category == null)
                throw new CategoryNotFoundException(
                    $"Could not found Category with id: {deleteCategoryCommand.Id} for deleting Category.");

            var items = await _itemRepository.GetAllAsync();
            var itemsAssignedToCategory = items.Where(i => i.Category.Id== category.Id);

            foreach(var item in itemsAssignedToCategory)
            {
                await _itemRepository.DeleteAsync(item);
            }

            await _categoryRepository.DeleteAsync(deleteCategoryCommand.Id);
        }
    }
}
