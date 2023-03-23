using Cart.Api.Api.Mappers.Abstractions;
using Cart.Api.Api.Models;
using Cart.Api.Core.Models;

namespace Cart.Api.Api.Mappers
{
    public class ItemMapper : IItemMapper
    {
        public ItemViewModel Map(Item item) => new ItemViewModel
        {
            Id = item.Id,
            Name = item.Name,
            Price = item.Price,
            Quantity = item.Quantity,
            Image = new ImageViewModel
            {
                AltText = item.Image.AltText,
                Url = item.Image.Url
            }
        };
    }
}
