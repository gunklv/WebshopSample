using Catalog.Application.Item.Commands.DeleteItem;
using Catalog.Application.Shared.Exceptions;
using Catalog.Domain.Abstractions;
using MediatR;

namespace Catalog.Application.Shared.DeleteItem
{
    internal class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand>
    {
        private readonly IItemRepository _itemRepository;

        public DeleteItemCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task Handle(DeleteItemCommand deleteItemCommand, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByIdAsync(deleteItemCommand.Id);
            if (item == null)
                throw new ItemNotFoundException(
                    $"Could not found Item with id: {deleteItemCommand.Id} for deleting Item.");

            await _itemRepository.DeleteAsync(deleteItemCommand.Id);
        }
    }
}
