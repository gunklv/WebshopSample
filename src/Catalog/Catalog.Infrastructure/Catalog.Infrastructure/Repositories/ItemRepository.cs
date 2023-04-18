using Catalog.Domain.Abstractions;
using Catalog.Domain.Abstractions.Filters;
using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.Configurations;
using Catalog.Infrastructure.Mappers.Abstractions;
using Catalog.Infrastructure.Models;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Catalog.Infrastructure.Repositories
{
    internal class ItemRepository : IItemRepository
    {
        private const string TableName = "item";
        private readonly NpgsqlConnection _npgsqlConnection;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IItemMapper _itemMapper;

        public ItemRepository(
            IOptions<PersistenceConfiguration> persistenceOptions,
            IRepository<Category> categoryRepository,
            IItemMapper itemMapper)
        {
            _npgsqlConnection = new NpgsqlConnection(persistenceOptions.Value.ConnectionString);
            _npgsqlConnection.Open();
            _categoryRepository = categoryRepository;
            _itemMapper = itemMapper;
        }

        public async Task<Item> InsertAsync(Item item)
        {
            var sql = $"INSERT INTO {TableName} (categoryId, name, description, imageUrl, price, amount) " +
                $"VALUES (@categoryId, @name, @description, @imageUrl, @price, @amount) RETURNING id";

            var queryArguments = new ItemDto
            {
                CategoryId = item.Category.Id,
                Name = item.Name,
                Description = item.Description,
                ImageUrl = item.ImageUrl,
                Price = item.Price,
                Amount = item.Amount
            };

            var id = await _npgsqlConnection.ExecuteScalarAsync<long>(sql, queryArguments);

            return new Item(id, item.Name, item.Description, item.ImageUrl, item.Price, item.Amount, item.Category);
        }

        public async Task<Item> GetByIdAsync(object id)
        {
            string commandText = $"SELECT * FROM {TableName} WHERE id = @id";

            var queryArguments = new { Id = id };
            var itemDto = await _npgsqlConnection.QueryFirstOrDefaultAsync<ItemDto>(commandText, queryArguments);

            if(itemDto == null)
            {
                return null;
            }

            Category category = null;
            if(itemDto.CategoryId != null)
            {
                category = await _categoryRepository.GetByIdAsync(itemDto.CategoryId);
            }

            var item = _itemMapper.Map(itemDto, category);

            return item;
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await GetAllAsync(null, null, null);
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync(Action<ItemFilter> itemFilterAction, int? page, int? limit)
        {
            string commandText = $"SELECT * FROM {TableName}";

            if (itemFilterAction != null)
            {
                var itemFilter = new ItemFilter();
                itemFilterAction.Invoke(itemFilter);
                if(itemFilter.CategoryId != null)
                {
                    commandText += $" WHERE CategoryId = '{itemFilter.CategoryId}'";
                }
            }

            if(page != null && page != 0 && limit != null && limit != 0)
            {
                commandText += $" LIMIT {limit} OFFSET {(page - 1) * limit}";
            }

            var itemDtos = (await _npgsqlConnection.QueryAsync<ItemDto>(commandText)).ToList();
            var items = new List<Item>();

            var categories = await _categoryRepository.GetAllAsync();

            foreach(var itemDto in itemDtos)
            {
                Category category = null;
                if(itemDto.CategoryId != null)
                {
                    category = categories.FirstOrDefault(c => c.Id == itemDto.CategoryId);
                }
                var item = _itemMapper.Map(itemDto, category);

                items.Add(item);
            }

            return items;
        }

        public async Task<Item> UpdateAsync(Item item)
        {
            var commandText = $@"UPDATE {TableName}
                SET categoryId = @categoryId, name = @name, description = @description, imageUrl = @imageUrl, price = @price, amount = @amount
                WHERE id = @id";

            var queryArguments = new
            {
                id = item.Id,
                categoryId = item.Category.Id,
                name = item.Name,
                description = item.Description,
                imageUrl = item.ImageUrl,
                price = item.Price,
                amount = item.Amount
            };

            await _npgsqlConnection.ExecuteAsync(commandText, queryArguments);

            return new Item(item.Id, item.Name, item.Description, item.ImageUrl, item.Price, item.Amount, item.Category);
        }

        public async Task DeleteAsync(object id)
        {
            string commandText = $"DELETE FROM {TableName} WHERE id=@id";

            var queryArguments = new { id };

            await _npgsqlConnection.ExecuteAsync(commandText, queryArguments);
        }
    }
}
