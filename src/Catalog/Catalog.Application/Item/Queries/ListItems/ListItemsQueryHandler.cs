using Catalog.Domain.Abstractions;
using MediatR;

namespace Catalog.Application.Item.Queries.ListItems
{
    internal class ListItemsQueryHandler : IRequestHandler<ListItemsQuery, IReadOnlyCollection<Domain.Aggregates.Item>>
    {
        private readonly IRepository<Domain.Aggregates.Item> _itemRepository;

        public ListItemsQueryHandler(IRepository<Domain.Aggregates.Item> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IReadOnlyCollection<Domain.Aggregates.Item>> Handle(ListItemsQuery listItemsQuery, CancellationToken cancellationToken)
        {
            return (await _itemRepository.GetAllAsync()).ToList();
        }
    }
}
