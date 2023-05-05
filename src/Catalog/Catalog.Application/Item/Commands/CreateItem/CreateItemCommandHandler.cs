using Catalog.Application.Shared;
using Catalog.Application.Shared.Exceptions;
using MediatR;

namespace Catalog.Application.Item.Commands.CreateItem
{
    internal class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Domain.ItemAggregate.Item>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Domain.ItemAggregate.Item> Handle(CreateItemCommand createItemCommand, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginAsync();
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(createItemCommand.CategoryId);
                if (category == null)
                    throw new CategoryNotFoundException(
                        $"Could not found Category with id: {createItemCommand.CategoryId} for Item creation.");

                var item = new Domain.ItemAggregate.Item(
                    null, createItemCommand.Name, createItemCommand.Description, createItemCommand.ImageUrl, createItemCommand.Price, createItemCommand.Amount, category.Id);

                var insertedItem = await _unitOfWork.ItemRepository.InsertAsync(item);

                await _unitOfWork.CommitAsync();

                return insertedItem;
            }
            catch(Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
