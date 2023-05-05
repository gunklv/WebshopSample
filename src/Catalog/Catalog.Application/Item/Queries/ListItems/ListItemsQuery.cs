using Catalog.Application.Shared.Models;
using MediatR;

namespace Catalog.Application.Item.Queries.ListItems
{
    public class ListItemsQuery : IRequest<IReadOnlyCollection<Domain.ItemAggregate.Item>>
    {
        public PagedQueryParams PagedQueryParams { get; set; }
        public ListItemsFilter ListItemsFilter { get; set; }
    }

    public class ListItemsFilter
    {
        public Guid? CategoryId { get; set; }
    }
}
