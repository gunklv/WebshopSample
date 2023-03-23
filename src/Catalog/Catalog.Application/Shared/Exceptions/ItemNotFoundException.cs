using Catalog.Application.Shared.Exceptions.Abstraction;

namespace Catalog.Application.Shared.Exceptions
{
    internal class ItemNotFoundException : NotFoundException
    {
        public ItemNotFoundException(string message) : base(message)
        {
        }
    }
}
