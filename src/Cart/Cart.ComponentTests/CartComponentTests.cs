using Cart.Api.Api.Models;
using Cart.ComponentTests.Infrastructure;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
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
            var firstAddItemToCartRequest = new AddItemToCartRequest
            {
                Name = "TestName",
                ItemId = firstItemId,
                Price = 100
            };
            var addItemToCartRequestContent = new StringContent(JsonConvert.SerializeObject(firstAddItemToCartRequest), Encoding.UTF8, "application/json");

            var secondItemId = 2;
            var secondAddItemToCartRequest = new AddItemToCartRequest
            {
                Name = "TestName",
                ItemId = secondItemId,
                Price = 100
            };
            var secondItemToCartRequestContent = new StringContent(JsonConvert.SerializeObject(secondAddItemToCartRequest), Encoding.UTF8, "application/json");

            var client = _factory.CreateClient();

            // Act && Assert

            // step1: adding first item to cart
            var addingSecondItemToCartResponse = await client.PostAsync($"api/v1/carts/{cartId}/items", addItemToCartRequestContent);
            addingSecondItemToCartResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, addingSecondItemToCartResponse.StatusCode);

            // step2: adding second item to cart
            var addingItemToCartResponse = await client.PostAsync($"api/v1/carts/{cartId}/items", secondItemToCartRequestContent);
            addingItemToCartResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, addingItemToCartResponse.StatusCode);

            // step3: getting item list
            var itemListFirstResponse = await client.GetAsync($"api/v1/carts/{cartId}/items");
            itemListFirstResponse.EnsureSuccessStatusCode();
            var firstRequestitemModels = await itemListFirstResponse.Content.ReadFromJsonAsync<GetCartItemListResponse>();
            Assert.Equal(2, firstRequestitemModels.ItemViewModelCollection.Count);
            Assert.Equal(firstItemId, firstRequestitemModels.ItemViewModelCollection[0].ItemId);
            Assert.Equal(secondItemId, firstRequestitemModels.ItemViewModelCollection[1].ItemId);

            // step4: removing first item from the cart
            var removingFirstItemResponse = await client.DeleteAsync($"api/v1/carts/{cartId}/items/{firstItemId}");
            removingFirstItemResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, removingFirstItemResponse.StatusCode);

            // step5: getting modified item list
            var itemListSecondResponse = await client.GetAsync($"api/v1/carts/{cartId}/items");
            itemListSecondResponse.EnsureSuccessStatusCode();
            var secondRequestItemModel = await itemListSecondResponse.Content.ReadFromJsonAsync<GetCartItemListResponse>();
            Assert.Single(secondRequestItemModel.ItemViewModelCollection);
            Assert.Equal(secondItemId, secondRequestItemModel.ItemViewModelCollection[0].ItemId);
        }
    }
}
