using Catalog.Domain.CategoryAggregate;
using Catalog.Infrastructure.Persistance.Sql.Mappers.Abstractions;
using Catalog.Infrastructure.Persistance.Sql.Models;

namespace Catalog.Infrastructure.Persistance.Sql.Mappers
{
    internal class CategoryMapper : ICategoryMapper
    {
        public Category Map(CategoryDto categoryDto, Category parentCategory)
        => new Category(categoryDto.Id, categoryDto.Name, categoryDto.ImageUrl, parentCategory);
    }
}
