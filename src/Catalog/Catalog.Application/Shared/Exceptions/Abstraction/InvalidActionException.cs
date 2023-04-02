namespace Catalog.Application.Shared.Exceptions.Abstraction
{
    public abstract class InvalidActionException : Exception
    {
        protected InvalidActionException(string message) : base(message)
        {
        }
    }
}
