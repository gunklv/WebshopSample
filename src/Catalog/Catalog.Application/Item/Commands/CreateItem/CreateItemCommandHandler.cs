using Catalog.Application.Shared.Exceptions;
using Catalog.Domain.Abstractions;
using MediatR;

namespace Catalog.Application.Item.Commands.CreateItem
{
    internal class CreateItemCommandHandler : IRequestHandler<CreateItemCommand>
    {
        private readonly IRepository<Domain.Aggregates.Item> _itemRepository;
        private readonly IRepository<Domain.Aggregates.Category> _categoryRepository;

        public CreateItemCommandHandler(
            IRepository<Domain.Aggregates.Item> itemRepository,
            IRepository<Domain.Aggregates.Category> categoryRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(CreateItemCommand createItemCommand, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(createItemCommand.CategoryId);
            if (category == null)
                throw new CategoryNotFoundException(
                    $"Could not found Category with id: {createItemCommand.CategoryId} for Item creation.");

            var item = new Domain.Aggregates.Item(
                null, createItemCommand.Name, createItemCommand.Description, createItemCommand.ImageUrl, createItemCommand.Price, createItemCommand.Amount, category);

            await _itemRepository.InsertAsync(item);
        }
    }
}
