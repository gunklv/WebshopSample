using Catalog.Domain.Abstractions;
using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.Configurations;
using Catalog.Infrastructure.Mappers.Abstractions;
using Catalog.Infrastructure.Models;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Catalog.Infrastructure.Repositories
{
    internal class CategoryRepository : IRepository<Category>
    {
        private const string TableName = "category";
        private readonly NpgsqlConnection _npgsqlConnection;
        private readonly ICategoryMapper _categoryMapper;

        public CategoryRepository(
            IOptions<PersistenceConfiguration> persistenceOptions,
            ICategoryMapper categoryMapper)
        {
            _npgsqlConnection = new NpgsqlConnection(persistenceOptions.Value.ConnectionString);
            _npgsqlConnection.Open();
            _categoryMapper = categoryMapper;
        }

        public async Task InsertAsync(Category category)
        {
            var sql = $"INSERT INTO {TableName} (parentId, name, imageUrl) VALUES (@parentId, @name, @imageUrl)";

            var queryArguments = new
            {
                parentId = category.ParentCategory?.Id,
                name = category.Name,
                imageUrl = category.ImageUrl
            };

            await _npgsqlConnection.ExecuteAsync(sql, queryArguments);
        }

        public async Task<Category> GetByIdAsync(object id)
        {
            string commandText = $"SELECT * FROM {TableName} WHERE id = @id";

            var queryArguments = new { Id = id };
            var categoryDto = await _npgsqlConnection.QueryFirstAsync<CategoryDto>(commandText, queryArguments);

            Category parentCategory = null;
            if(categoryDto.ParentId != null)
            {
                parentCategory = await GetByIdAsync(categoryDto.ParentId);
            }

            var category = _categoryMapper.Map(categoryDto, parentCategory);

            return category;
        }

        public async Task<IReadOnlyCollection<Category>> GetAllAsync()
        {
            string commandText = $"SELECT * FROM {TableName}";

            var categoryDtos = (await _npgsqlConnection.QueryAsync<CategoryDto>(commandText)).ToList();
            var categories = new List<Category>();

            foreach(var categoryDto in categoryDtos)
            {
                Category parentCategory = null;
                if(categoryDto.ParentId != null)
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

        public async Task UpdateAsync(Category category)
        {
            var commandText = $@"UPDATE {TableName}
                SET parentId = @parentId, name = @name, imageUrl = @imageUrl
                WHERE id = @id";

            var queryArguments = new
            {
                id = category.Id,
                parentId = category.ParentCategory.Id,
                name = category.Name,
                imageUrl = category.ImageUrl
            };

            await _npgsqlConnection.ExecuteAsync(commandText, queryArguments);
        }

        public async Task DeleteAsync(object id)
        {
            string commandText = $"DELETE FROM {TableName} WHERE id=@id";

            var queryArguments = new { id };

            await _npgsqlConnection.ExecuteAsync(commandText, queryArguments);
        }
    }
}
