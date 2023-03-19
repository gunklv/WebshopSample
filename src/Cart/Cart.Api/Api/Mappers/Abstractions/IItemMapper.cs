using Cart.Api.Api.Models;
using Cart.Api.Core.Models;

namespace Cart.Api.Api.Mappers.Abstractions
{
    public interface IItemMapper
    {
        ItemModel Map(Item item);
    }
}
