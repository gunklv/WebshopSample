using Catalog.Domain.CategoryAggregate;
using Catalog.Infrastructure.Persistance.Sql.Models;

namespace Catalog.Infrastructure.Persistance.Sql.Mappers.Abstractions
{
    internal interface ICategoryMapper
    {
        public Category Map(CategoryDto categoryDto, Category parentCategory);
    }
}
