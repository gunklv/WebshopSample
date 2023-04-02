using Catalog.Application.Shared.Exceptions;
using Catalog.Domain.Abstractions;
using MediatR;

namespace Catalog.Application.Category.Commands.DeleteCategory
{
    internal class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IRepository<Domain.Aggregates.Category> _categoryRepository;
        private readonly IRepository<Domain.Aggregates.Item> _itemRepository;

        public DeleteCategoryCommandHandler(
            IRepository<Domain.Aggregates.Category> categoryRepository,
            IRepository<Domain.Aggregates.Item> itemRepository)
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

            if(items.Any(i => i.Category.Id== category.Id))
                throw new CategoryCanNotBeDeletedException($"Category with id {category.Id} are assigned to Item(s).");

            await _categoryRepository.DeleteAsync(deleteCategoryCommand.Id);
        }
    }
}
