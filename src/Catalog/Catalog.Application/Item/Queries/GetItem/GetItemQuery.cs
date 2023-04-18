using MediatR;

namespace Catalog.Application.Item.Queries.GetItem
{
    public class GetItemQuery : IRequest<Domain.ItemAggregate.Item>
    {
        public long Id { get; set; }
    }
}
