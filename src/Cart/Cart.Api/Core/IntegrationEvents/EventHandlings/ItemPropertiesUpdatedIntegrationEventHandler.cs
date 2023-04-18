using Cart.Api.Core.Abstractions.Repositories;
using Cart.Api.Core.IntegrationEvents.EventHandlings.Abstractions;
using Cart.Api.Core.IntegrationEvents.Events;

namespace Cart.Api.Core.IntegrationEvents.EventHandlings
{
    public class ItemPropertiesUpdatedIntegrationEventHandler : IIntegrationEventHandler<ItemPropertiesUpdatedIntegrationEvent>
    {
        private readonly ICartRepository _cartRepository;

        public ItemPropertiesUpdatedIntegrationEventHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task HandleAsync(ItemPropertiesUpdatedIntegrationEvent @event)
        {
            var cartCollection = await _cartRepository.GetAllCartsAsync();
            
            var cartsWithModifiedItems = cartCollection.Where(x => x.ItemList.Any(y => y.Id == @event.ItemId));

            foreach(var cart in cartsWithModifiedItems)
            {
                var item = cart.ItemList.FirstOrDefault(x => x.Id == @event.ItemId);

                item.Name = @event.Name;
                item.Price = @event.Price;
                item.Image.Url = @event.ImageUrl;

                await _cartRepository.UpdateCartAsync(cart);
            }
        }
    }
}
