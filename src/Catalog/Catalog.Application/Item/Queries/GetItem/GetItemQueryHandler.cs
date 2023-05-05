using Catalog.Application.Shared;
using Catalog.Application.Shared.Exceptions;
using MediatR;

namespace Catalog.Application.Item.Queries.GetItem
{
    internal class GetItemQueryHandler : IRequestHandler<GetItemQuery, Domain.ItemAggregate.Item>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetItemQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Domain.ItemAggregate.Item> Handle(GetItemQuery getItemQuery, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginAsync();
            try
            {
                var item = await _unitOfWork.ItemRepository.GetByIdAsync(getItemQuery.Id);
                if (item == null)
                    throw new ItemNotFoundException(
                        $"Could not found Item with id: {getItemQuery.Id}.");

                return item;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
