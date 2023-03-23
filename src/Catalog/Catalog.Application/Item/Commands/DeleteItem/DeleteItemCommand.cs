using MediatR;

namespace Catalog.Application.Item.Commands.DeleteItem
{
    public class DeleteItemCommand : IRequest
    {
        public long Id { get; set; }
    }
}
