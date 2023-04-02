using Cart.Api.Api.Mappers.Abstractions;
using Cart.Api.Api.Models;
using Cart.Api.Core.Models;

namespace Cart.Api.Api.Mappers
{
    public class ItemMapper : IItemMapper
    {
        public ItemViewModel Map(Item item) => new ItemViewModel
        {
            ItemId = item.Id,
            Name = item.Name,
            Price = item.Price,
            Quantity = item.Quantity,
            ImageAltText = item.Image.AltText,
            ImageUrl = item.Image.Url
        };

        public Item Map(AddItemToCartRequest addItemToCartRequest) => new Item
        {
            Id = addItemToCartRequest.ItemId,
            Name = addItemToCartRequest.Name,
            Price = addItemToCartRequest.Price,
            Quantity = 1,
            Image = new Image
            {
                AltText = addItemToCartRequest.ImageAltText,
                Url = addItemToCartRequest.ImageUrl
            }
        };
    }
}
