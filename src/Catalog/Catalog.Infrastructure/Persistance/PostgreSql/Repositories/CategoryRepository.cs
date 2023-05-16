﻿using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Shared.Repositories;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Mappers.Abstractions;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Models;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Utils;
using Dapper;
using Npgsql;
using System.Data;

namespace Catalog.Infrastructure.Persistance.PostgreSql.Repositories
{
    internal class CategoryRepository : IRepository<Category>
    {
        private const string TABLE_NAME = "category";

        private readonly NpgsqlConnection _npgsqlConnection;
        private readonly IChangeTracker _changeTracker;
        private readonly IDbTransaction _dbTransaction;
        private readonly ICategoryMapper _categoryMapper;

        public CategoryRepository(
            NpgsqlConnection npgsqlConnection,
            IChangeTracker changeTracker,
            IDbTransaction dbTransaction,
            ICategoryMapper categoryMapper)
        {
            _npgsqlConnection = npgsqlConnection;
            _changeTracker = changeTracker;
            _dbTransaction = dbTransaction;
            _categoryMapper = categoryMapper;
        }

        public async Task<Category> InsertAsync(Category category)
        {
            var sql = $"INSERT INTO {TABLE_NAME} (parentId, name, imageUrl) VALUES (@parentId, @name, @imageUrl) RETURNING id";

            var queryArguments = new CategoryDto
            {
                ParentId = category.ParentCategory?.Id,
                Name = category.Name,
                ImageUrl = category.ImageUrl
            };

            var id = await _npgsqlConnection.ExecuteScalarAsync<Guid>(sql, queryArguments, _dbTransaction);

            _changeTracker.AddChangedAggregateRoot(category);

            // Id setter is private by default, so reflection is used to set it after Id has been generated by the db
            var prop = category.GetType().GetProperty(nameof(category.Id));
            prop.SetValue(category, id);

            return category;
        }

        public async Task<Category> GetByIdAsync(object id)
        {
            string commandText = $"SELECT * FROM {TABLE_NAME} WHERE id = @id";

            var queryArguments = new { Id = id };
            var categoryDto = await _npgsqlConnection.QueryFirstOrDefaultAsync<CategoryDto>(commandText, queryArguments, _dbTransaction);

            if (categoryDto == null)
            {
                return null;
            }

            Category parentCategory = null;
            if (categoryDto.ParentId != null)
            {
                parentCategory = await GetByIdAsync(categoryDto.ParentId);
            }

            var category = _categoryMapper.Map(categoryDto, parentCategory);

            return category;
        }

        public async Task<IReadOnlyCollection<Category>> GetAllAsync()
        {
            string commandText = $"SELECT * FROM {TABLE_NAME}";

            var categoryDtos = (await _npgsqlConnection.QueryAsync<CategoryDto>(commandText, _dbTransaction)).ToList();
            var categories = new List<Category>();

            foreach (var categoryDto in categoryDtos)
            {
                Category parentCategory = null;
                if (categoryDto.ParentId != null)
                {
                    parentCategory = InMemoryGetById(categoryDto.ParentId.Value, categoryDtos);
                }
                var category = _categoryMapper.Map(categoryDto, parentCategory);

                categories.Add(category);
            }

            return categories;
        }

        private Category InMemoryGetById(Guid id, IReadOnlyCollection<CategoryDto> inMemoryCategoryDtoCollection)
        {
            var categoryDto = inMemoryCategoryDtoCollection.FirstOrDefault(c => c.Id == id);

            Category parentCategory = null;
            if (categoryDto.ParentId != null)
            {
                parentCategory = InMemoryGetById(categoryDto.ParentId.Value, inMemoryCategoryDtoCollection);
            }

            var category = _categoryMapper.Map(categoryDto, parentCategory);

            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var commandText = $@"UPDATE {TABLE_NAME}
                SET parentId = @parentId, name = @name, imageUrl = @imageUrl
                WHERE id = @id";

            var queryArguments = new
            {
                id = category.Id,
                parentId = category.ParentCategory?.Id,
                name = category.Name,
                imageUrl = category.ImageUrl
            };

            await _npgsqlConnection.ExecuteAsync(commandText, queryArguments, _dbTransaction);

            _changeTracker.AddChangedAggregateRoot(category);

            return category;
        }

        public async Task DeleteAsync(Category category)
        {
            string commandText = $"DELETE FROM {TABLE_NAME} WHERE id=@id";

            var queryArguments = new { category.Id };

            await _npgsqlConnection.ExecuteAsync(commandText, queryArguments, _dbTransaction);

            _changeTracker.AddChangedAggregateRoot(category);
        }
    }
}