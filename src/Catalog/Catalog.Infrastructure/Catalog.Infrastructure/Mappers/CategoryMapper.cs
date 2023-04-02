using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.Mappers.Abstractions;
using Catalog.Infrastructure.Models;

namespace Catalog.Infrastructure.Mappers
{
    internal class CategoryMapper : ICategoryMapper
    {
        public Category Map(CategoryDto categoryDto, Category parentCategory)
        => new Category(categoryDto.Id, categoryDto.Name, categoryDto.ImageUrl, parentCategory);
    }
}
