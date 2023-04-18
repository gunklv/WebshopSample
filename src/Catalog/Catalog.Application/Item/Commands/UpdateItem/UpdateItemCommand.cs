using MediatR;

namespace Catalog.Application.Item.Commands.UpdateItem
{
    public class UpdateItemCommand : IRequest<Domain.Aggregates.Item>
    {
        public long Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Amount { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
