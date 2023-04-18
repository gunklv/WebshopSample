using Catalog.Domain.Abstractions;
using MediatR;

namespace Catalog.Application.Item.Queries.ListItems
{
    internal class ListItemsQueryHandler : IRequestHandler<ListItemsQuery, IReadOnlyCollection<Domain.Aggregates.Item>>
    {
        private readonly IItemRepository _itemRepository;

        public ListItemsQueryHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IReadOnlyCollection<Domain.Aggregates.Item>> Handle(ListItemsQuery listItemsQuery, CancellationToken cancellationToken)
        {
            var items = (await _itemRepository.GetAllAsync(
                itemFilterAction: x => x.CategoryId = listItemsQuery.ListItemsFilter?.CategoryId,
                page: listItemsQuery.PagedQueryParams.Page,
                limit: listItemsQuery.PagedQueryParams.Limit)).ToList();

            return items;
        }
    }
}
