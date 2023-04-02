using Cart.Api.Api.Models;
using Cart.ComponentTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Cart.ComponentTests
{
    public class CartComponentTests : IClassFixture<CartWebApplicationFactory>
    {
        private readonly CartWebApplicationFactory _factory;

        public CartComponentTests(CartWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Scenario_AddingTwoItems_Then_ReadingTwoItems_Then_RemovingOneItem_Then_ReadingOneItem_FromCart_WorksProperly()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var firstItemId = 1;
            var secondItemId = 2;
            var client = _factory.CreateClient();

            // Act && Assert

            // step1: adding first item to cart
            var addingSecondItemToCartResponse = await client.PostAsync($"carts/{cartId}/items/{firstItemId}", null);
            addingSecondItemToCartResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, addingSecondItemToCartResponse.StatusCode);

            // step2: adding second item to cart
            var addingItemToCartResponse = await client.PostAsync($"carts/{cartId}/items/{secondItemId}", null);
            addingItemToCartResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, addingItemToCartResponse.StatusCode);

            // step3: getting item list
            var itemListFirstResponse = await client.GetAsync($"carts/{cartId}/items");
            itemListFirstResponse.EnsureSuccessStatusCode();
            var firstRequestitemModels = await itemListFirstResponse.Content.ReadFromJsonAsync<List<ItemModel>>();
            Assert.Equal(2, firstRequestitemModels.Count);
            Assert.Equal(firstItemId, firstRequestitemModels[0].Id);
            Assert.Equal(secondItemId, firstRequestitemModels[1].Id);

            // step4: removing first item from the cart
            var removingFirstItemResponse = await client.DeleteAsync($"carts/{cartId}/items/{firstItemId}");
            removingFirstItemResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, removingFirstItemResponse.StatusCode);

            // step5: getting modified item list
            var itemListSecondResponse = await client.GetAsync($"carts/{cartId}/items");
            itemListSecondResponse.EnsureSuccessStatusCode();
            var secondRequestItemModel = await itemListSecondResponse.Content.ReadFromJsonAsync<List<ItemModel>>();
            Assert.Single(secondRequestItemModel);
            Assert.Equal(secondItemId, secondRequestItemModel[0].Id);
        }
    }
}
