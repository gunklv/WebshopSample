using Catalog.Application.Shared.Exceptions;
using Catalog.Domain.Abstractions;
using MediatR;

namespace Catalog.Application.Item.Commands.UpdateItem
{
    internal class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Domain.Aggregates.Item>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IRepository<Domain.Aggregates.Category> _categoryRepository;

        public UpdateItemCommandHandler(
            IItemRepository itemRepository,
            IRepository<Domain.Aggregates.Category> categoryRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Domain.Aggregates.Item> Handle(UpdateItemCommand updateItemCommand, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByIdAsync(updateItemCommand.Id);
            if (item == null)
                throw new ItemNotFoundException(
                    $"Could not found Item with id: {updateItemCommand.Id} for updating Item.");

            var category = await _categoryRepository.GetByIdAsync(updateItemCommand.CategoryId);
            if (category == null)
                throw new CategoryNotFoundException(
                    $"Could not found Category with id: {updateItemCommand.CategoryId} for updating Item with id: {updateItemCommand.Id}.");

            item.Name = updateItemCommand.Name;
            item.Description = updateItemCommand.Description;
            item.ImageUrl = updateItemCommand.ImageUrl;
            item.Price= updateItemCommand.Price;
            item.Amount = updateItemCommand.Amount;
            item.Category = category;

            return await _itemRepository.UpdateAsync(item);
        }
    }
}
