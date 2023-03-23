using Cart.Api.Core.Abstractions.Repositories;
using Cart.Api.Core.Exceptions;
using Cart.Api.Core.Models;
using Cart.Api.Core.Services;
using Cart.Api.Core.Services.Abstractions;
using Moq;
using Xunit;
using Domain = Cart.Api.Core.Models;

namespace Cart.UnitTests.Core.Services
{
    public class CartServiceTest
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock = new Mock<ICartRepository>();
        private readonly ICartService _cartService;

        public CartServiceTest()
        {
            _cartService = new CartService(_cartRepositoryMock.Object);
        }

        [Fact]
        public async Task AddItemToCartAsync_WhenNoCartAvailable_CreatesCartWithItem()
        {
            // Arrange
            var itemId = 1;
            var cartId = Guid.NewGuid();

            _cartRepositoryMock.Setup(x => x.GetCartAsync(cartId)).Returns(Task.FromResult(default(Domain.Cart)));

            // Act
            await _cartService.AddItemToCartAsync(cartId, itemId);

            // Assert
            _cartRepositoryMock.Verify(x => x.InsertCartAsync(It.Is<Domain.Cart>(x => x.ItemList.First().Id == itemId)), Times.Once);
        }

        [Fact]
        public async Task AddItemToCartAsync_WhenCartAvailable_WithNoItem_UpdatesExistingCartWithItem()
        {
            // Arrange
            var itemId = 1;
            var cart = new Domain.Cart { Id = Guid.NewGuid() };

            _cartRepositoryMock.Setup(x => x.GetCartAsync(cart.Id)).Returns(Task.FromResult(cart));

            // Act
            await _cartService.AddItemToCartAsync(cart.Id, itemId);

            // Assert
            _cartRepositoryMock.Verify(x => x.InsertCartAsync(It.IsAny<Domain.Cart>()), Times.Never);
            _cartRepositoryMock.Verify(x => x.UpdateCartAsync(cart), Times.Once);
            Assert.True(cart.ItemList.First().Id == itemId);
            Assert.True(cart.ItemList.First().Quantity == 1);
        }

        [Fact]
        public async Task AddItemToCartAsync_WhenCartAvailable_WithOneSameItem_IncreasesItsQuantity()
        {
            // Arrange
            var item = new Item { Id = 1, Quantity = 1 };
            var cart = new Domain.Cart { Id = Guid.NewGuid(), ItemList = new List<Item> { item } };

            _cartRepositoryMock.Setup(x => x.GetCartAsync(cart.Id)).Returns(Task.FromResult(cart));

            // Act
            await _cartService.AddItemToCartAsync(cart.Id, item.Id);

            // Assert
            _cartRepositoryMock.Verify(x => x.UpdateCartAsync(cart), Times.Once);
            Assert.True(cart.ItemList.First().Id == item.Id);
            Assert.Equal(2, item.Quantity);
        }

        [Fact]
        public async Task GetCartItemListAsync_WithItemAssignedToCart_ReturnsItem()
        {
            // Arrange
            var item = new Item { Id = 1, Quantity = 1 };
            var cart = new Domain.Cart { Id = Guid.NewGuid(), ItemList = new List<Item> { item } };

            _cartRepositoryMock.Setup(x => x.GetCartAsync(cart.Id)).Returns(Task.FromResult(cart));

            // Act
            var itemList = await _cartService.GetCartItemListAsync(cart.Id);

            // Assert
            Assert.True(itemList.First() == item);
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_WhenNoCartAvailable_ThrowsCartNotFoundException()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            _cartRepositoryMock.Setup(x => x.GetCartAsync(cartId)).Returns(Task.FromResult(default(Domain.Cart)));

            // Act & Assert
            await Assert.ThrowsAsync<CartNotFoundException>(async () => await _cartService.RemoveItemFromCartAsync(cartId, 0));
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_WhenCartAvailable_WithMultipleSameItems_DecreasesItsQuantity()
        {
            // Arrange
            var item = new Item { Id = 1, Quantity = 2 };
            var cart = new Domain.Cart { Id = Guid.NewGuid(), ItemList = new List<Item> { item } };

            _cartRepositoryMock.Setup(x => x.GetCartAsync(cart.Id)).Returns(Task.FromResult(cart));

            // Act
            await _cartService.RemoveItemFromCartAsync(cart.Id, item.Id);

            // Assert
            _cartRepositoryMock.Verify(x => x.UpdateCartAsync(cart), Times.Once);
            Assert.True(cart.ItemList.First().Id == item.Id);
            Assert.Equal(1, item.Quantity);
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_WhenCartAvailable_WithOneSameItem_DeletesCart()
        {
            // Arrange
            var item = new Item { Id = 1, Quantity = 1 };
            var cart = new Domain.Cart { Id = Guid.NewGuid(), ItemList = new List<Item> { item } };

            _cartRepositoryMock.Setup(x => x.GetCartAsync(cart.Id)).Returns(Task.FromResult(cart));

            // Act
            await _cartService.RemoveItemFromCartAsync(cart.Id, item.Id);

            // Assert
            _cartRepositoryMock.Verify(x => x.DeleteCartAsync(cart.Id), Times.Once);
            Assert.False(cart.ItemList.Any());
        }
    }
}
