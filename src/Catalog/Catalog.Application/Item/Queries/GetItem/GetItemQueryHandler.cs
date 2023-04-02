using Catalog.Application.Shared.Exceptions;
using Catalog.Domain.Abstractions;
using MediatR;

namespace Catalog.Application.Item.Queries.GetItem
{
    internal class GetItemQueryHandler : IRequestHandler<GetItemQuery, Domain.Aggregates.Item>
    {
        private readonly IRepository<Domain.Aggregates.Item> _itemRepository;

        public GetItemQueryHandler(IRepository<Domain.Aggregates.Item> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<Domain.Aggregates.Item> Handle(GetItemQuery getItemQuery, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByIdAsync(getItemQuery.Id);
            if (item == null)
                throw new ItemNotFoundException(
                    $"Could not found Item with id: {getItemQuery.Id}.");

            return item;
        }
    }
}
