using MediatR;

namespace Catalog.Application.Item.Commands.CreateItem
{
    public class CreateItemCommand : IRequest
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Amount { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
