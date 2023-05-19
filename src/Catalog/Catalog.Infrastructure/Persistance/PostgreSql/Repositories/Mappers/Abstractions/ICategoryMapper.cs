using Catalog.Domain.CategoryAggregate;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Models;

namespace Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Mappers.Abstractions
{
    internal interface ICategoryMapper
    {
        public Category Map(CategoryDto categoryDto, Category parentCategory);
    }
}
