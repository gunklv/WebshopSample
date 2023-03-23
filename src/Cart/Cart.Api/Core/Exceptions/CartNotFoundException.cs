namespace Cart.Api.Core.Exceptions
{
    public class CartNotFoundException : Exception
    {
        public CartNotFoundException(string message) : base(message)
        {
        }
    }
}
