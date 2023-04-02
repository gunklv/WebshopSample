using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.Models;

namespace Catalog.Infrastructure.Mappers.Abstractions
{
    internal interface ICategoryMapper
    {
        public Category Map(CategoryDto categoryDto, Category parentCategory);
    }
}
