using MediatR;

namespace Catalog.Application.Item.Queries.ListItems
{
    public class ListItemsQuery : IRequest<IReadOnlyCollection<Domain.Aggregates.Item>>
    {
    }
}
