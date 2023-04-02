using Cart.Api.Core.Abstractions.Repositories;
using Cart.Api.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Domain = Cart.Api.Core.Models;

namespace Cart.Api.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<Domain.Cart> _collection;

        public CartRepository(IOptions<PersistenceConfiguration> configuration)
        {
            BsonClassMap.RegisterClassMap<Domain.Cart>(m =>
            {
                m.AutoMap();
                m.MapIdMember(d => d.Id).SetSerializer(new GuidSerializer(BsonType.String));
            });

            var database = new MongoClient(configuration.Value.ConnectionString).GetDatabase(configuration.Value.DatabaseName);
            _collection = database.GetCollection<Domain.Cart>(nameof(Domain.Cart));
        }

        public async Task InsertCartAsync(Domain.Cart cart)
        {
            await _collection.InsertOneAsync(cart);
        }

        public async Task<Domain.Cart> GetCartAsync(Guid cartId)
        {
            var cursor = await _collection.FindAsync(c => c.Id == cartId);
            return cursor.FirstOrDefault();
        }

        public async Task UpdateCartAsync(Domain.Cart cart)
        {
            await _collection.ReplaceOneAsync(c => c.Id == cart.Id, cart, new ReplaceOptions { IsUpsert = false });
        }

        public async Task DeleteCartAsync(Guid cartId)
        {
            await _collection.DeleteOneAsync(c => c.Id == cartId);
        }
    }
}
