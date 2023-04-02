namespace Cart.Api.Core.Models.Exceptions
{
    public class CartNotFoundException : Exception
    {
        public CartNotFoundException(string message) : base(message)
        {
        }
    }
}
