using Catalog.Application.Item.Commands.DeleteItem;
using Catalog.Application.Shared.Exceptions;
using MediatR;

namespace Catalog.Application.Shared.DeleteItem
{
    internal class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteItemCommand deleteItemCommand, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginAsync();
            try
            {
                var item = await _unitOfWork.ItemRepository.GetByIdAsync(deleteItemCommand.Id);
                if (item == null)
                    throw new ItemNotFoundException(
                        $"Could not found Item with id: {deleteItemCommand.Id} for deleting Item.");

                await _unitOfWork.ItemRepository.DeleteAsync(item);
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
