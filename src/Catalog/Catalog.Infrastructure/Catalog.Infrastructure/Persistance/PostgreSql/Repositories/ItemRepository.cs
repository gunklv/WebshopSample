﻿using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.ItemAggregate;
using Catalog.Domain.Shared.Repositories;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Mappers.Abstractions;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Models;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Utils;
using Dapper;
using Npgsql;
using System.Data;

namespace Catalog.Infrastructure.Persistance.PostgreSql.Repositories
{
    internal class ItemRepository : IItemRepository
    {
        private const string TABLE_NAME = "item";

        private readonly NpgsqlConnection _npgsqlConnection;
        private readonly IChangeTracker _changeTracker;
        private readonly IDbTransaction _dbTransaction;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IItemMapper _itemMapper;

        public ItemRepository(
            NpgsqlConnection npgsqlConnection,
            IChangeTracker changeTracker,
            IDbTransaction dbTransaction,
            IRepository<Category> categoryRepository,
            IItemMapper itemMapper)
        {
            _npgsqlConnection = npgsqlConnection;
            _changeTracker = changeTracker;
            _dbTransaction = dbTransaction;
            _categoryRepository = categoryRepository;
            _itemMapper = itemMapper;
        }

        public async Task<Item> InsertAsync(Item item)
        {
            var sql = $"INSERT INTO {TABLE_NAME} (categoryId, name, description, imageUrl, price, amount) " +
                $"VALUES (@categoryId, @name, @description, @imageUrl, @price, @amount) RETURNING id";

            var queryArguments = new ItemDto
            {
                CategoryId = item.CategoryId,
                Name = item.Name,
                Description = item.Description,
                ImageUrl = item.ImageUrl,
                Price = item.Price,
                Amount = item.Amount
            };

            var id = await _npgsqlConnection.ExecuteScalarAsync<long>(sql, queryArguments, _dbTransaction);

            _changeTracker.AddChangedAggregateRoot(item);

            // Id setter is private by default, so reflection is used to set it after Id has been generated by the db
            var prop = item.GetType().GetProperty(nameof(item.Id));
            prop.SetValue(item, id);

            return item;
        }

        public async Task<Item> GetByIdAsync(object id)
        {
            string commandText = $"SELECT * FROM {TABLE_NAME} WHERE id = @id";

            var queryArguments = new { Id = id };
            var itemDto = await _npgsqlConnection.QueryFirstOrDefaultAsync<ItemDto>(commandText, queryArguments, _dbTransaction);

            if (itemDto == null)
            {
                return null;
            }

            var item = _itemMapper.Map(itemDto);

            return item;
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await GetAllAsync(null, null, null);
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync(Action<ItemFilter> itemFilterAction, int? page, int? limit)
        {
            string commandText = $"SELECT * FROM {TABLE_NAME}";

            if (itemFilterAction != null)
            {
                var itemFilter = new ItemFilter();
                itemFilterAction.Invoke(itemFilter);
                if (itemFilter.CategoryId != null)
                {
                    commandText += $" WHERE CategoryId = '{itemFilter.CategoryId}'";
                }
            }

            if (page != null && page != 0 && limit != null && limit != 0)
            {
                commandText += $" LIMIT {limit} OFFSET {(page - 1) * limit}";
            }

            var itemDtos = (await _npgsqlConnection.QueryAsync<ItemDto>(commandText, _dbTransaction)).ToList();
            var items = new List<Item>();

            foreach (var itemDto in itemDtos)
            {
                var item = _itemMapper.Map(itemDto);
                items.Add(item);
            }

            return items;
        }

        public async Task<Item> UpdateAsync(Item item)
        {
            var commandText = $@"UPDATE {TABLE_NAME}
                SET categoryId = @categoryId, name = @name, description = @description, imageUrl = @imageUrl, price = @price, amount = @amount
                WHERE id = @id";

            var queryArguments = new
            {
                id = item.Id,
                categoryId = item.CategoryId,
                name = item.Name,
                description = item.Description,
                imageUrl = item.ImageUrl,
                price = item.Price,
                amount = item.Amount
            };

            await _npgsqlConnection.ExecuteAsync(commandText, queryArguments, _dbTransaction);

            _changeTracker.AddChangedAggregateRoot(item);

            return item;
        }

        public async Task DeleteAsync(Item item)
        {
            string commandText = $"DELETE FROM {TABLE_NAME} WHERE id=@id";

            var queryArguments = new { item.Id };

            await _npgsqlConnection.ExecuteAsync(commandText, queryArguments, _dbTransaction);

            _changeTracker.AddChangedAggregateRoot(item);
        }
    }
}