using Catalog.Application.Shared;
using Catalog.Application.Shared.Exceptions;
using MediatR;

namespace Catalog.Application.Item.Commands.UpdateItem
{
    internal class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Domain.ItemAggregate.Item>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Domain.ItemAggregate.Item> Handle(UpdateItemCommand updateItemCommand, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginAsync();
            try
            {
                var item = await _unitOfWork.ItemRepository.GetByIdAsync(updateItemCommand.Id);
                if (item == null)
                    throw new ItemNotFoundException(
                        $"Could not found Item with id: {updateItemCommand.Id} for updating Item.");

                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(updateItemCommand.CategoryId);
                if (category == null)
                    throw new CategoryNotFoundException(
                        $"Could not found Category with id: {updateItemCommand.CategoryId} for updating Item with id: {updateItemCommand.Id}.");

                item.UpdateProperties(
                    updateItemCommand.Name,
                    updateItemCommand.Description,
                    updateItemCommand.ImageUrl,
                    updateItemCommand.Price,
                    updateItemCommand.Amount,
                    category.Id);

                var updatedItem = await _unitOfWork.ItemRepository.UpdateAsync(item);

                await _unitOfWork.CommitAsync();

                return updatedItem;
            }
            catch(Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
