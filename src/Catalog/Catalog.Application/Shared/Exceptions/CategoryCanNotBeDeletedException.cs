using Catalog.Application.Shared.Exceptions.Abstraction;

namespace Catalog.Application.Shared.Exceptions
{
    internal class CategoryCanNotBeDeletedException : InvalidActionException
    {
        public CategoryCanNotBeDeletedException(string message) : base(message)
        {
        }
    }
}
