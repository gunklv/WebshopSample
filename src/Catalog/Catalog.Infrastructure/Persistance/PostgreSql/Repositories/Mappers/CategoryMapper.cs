using Catalog.Domain.CategoryAggregate;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Mappers.Abstractions;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Models;

namespace Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Mappers
{
    internal class CategoryMapper : ICategoryMapper
    {
        public Category Map(CategoryDto categoryDto, Category parentCategory)
        => new Category(categoryDto.Id, categoryDto.Name, categoryDto.ImageUrl, parentCategory);
    }
}
