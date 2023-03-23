using Catalog.Application.Shared.Exceptions.Abstraction;

namespace Catalog.Application.Shared.Exceptions
{
    internal class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(string message) : base(message)
        {
        }
    }
}
