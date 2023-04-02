namespace Cart.Api.Core.Validators.Abstractions
{
    public interface IDomainValidator<T> where T : class
    {
        Task EnsureValidityAsync(T message);
    }
}
