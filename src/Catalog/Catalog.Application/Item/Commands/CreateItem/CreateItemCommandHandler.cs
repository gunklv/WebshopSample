using Catalog.Application.Shared.Exceptions;
using Catalog.Domain.Abstractions;
using MediatR;

namespace Catalog.Application.Item.Commands.CreateItem
{
    internal class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Domain.Aggregates.Item>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IRepository<Domain.Aggregates.Category> _categoryRepository;

        public CreateItemCommandHandler(
            IItemRepository itemRepository,
            IRepository<Domain.Aggregates.Category> categoryRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Domain.Aggregates.Item> Handle(CreateItemCommand createItemCommand, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(createItemCommand.CategoryId);
            if (category == null)
                throw new CategoryNotFoundException(
                    $"Could not found Category with id: {createItemCommand.CategoryId} for Item creation.");

            var item = new Domain.Aggregates.Item(
                null, createItemCommand.Name, createItemCommand.Description, createItemCommand.ImageUrl, createItemCommand.Price, createItemCommand.Amount, category);

            return await _itemRepository.InsertAsync(item);
        }
    }
}
