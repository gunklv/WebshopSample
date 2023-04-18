using Catalog.Api.Models.Category.Requests;
using Catalog.Api.Models.Category.ViewModels;
using Catalog.Application.Category.Commands.CreateCategory;
using Catalog.Application.Category.Commands.UpdateCategory;
using Catalog.Domain.CategoryAggregate;

namespace Catalog.Api.Mappers.Abstractions
{
    public interface ICategoryMapper
    {
        CreateCategoryCommand Map(CreateCategoryRequest createCategoryRequest);
        UpdateCategoryCommand Map(Guid categoryId, UpdateCategoryRequest updateCategoryRequest);
        CategoryViewModel Map(Category category);
    }
}
