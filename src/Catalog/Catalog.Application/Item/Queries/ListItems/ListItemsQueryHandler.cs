using Catalog.Application.Shared;
using MediatR;

namespace Catalog.Application.Item.Queries.ListItems
{
    internal class ListItemsQueryHandler : IRequestHandler<ListItemsQuery, IReadOnlyCollection<Domain.ItemAggregate.Item>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListItemsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyCollection<Domain.ItemAggregate.Item>> Handle(ListItemsQuery listItemsQuery, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginAsync();
            try
            {
                var items = (await _unitOfWork.ItemRepository.GetAllAsync(
                itemFilterAction: x => x.CategoryId = listItemsQuery.ListItemsFilter?.CategoryId,
                page: listItemsQuery.PagedQueryParams.Page,
                limit: listItemsQuery.PagedQueryParams.Limit)).ToList();

                return items;
            }
            catch(Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
