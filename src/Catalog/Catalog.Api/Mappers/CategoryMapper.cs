using Catalog.Domain.Aggregates;
using Catalog.Api.Models.Category.Requests;
using Catalog.Application.Category.Commands.UpdateCategory;
using Catalog.Application.Category.Commands.CreateCategory;
using Catalog.Api.Models.Category.ViewModels;
using Catalog.Api.Mappers.Abstractions;
namespace Catalog.Api.Mappers
{
    public class CategoryMapper : ICategoryMapper
    {
        public CreateCategoryCommand Map(CreateCategoryRequest createCategoryRequest)
        => new CreateCategoryCommand
        {
            ParentId = createCategoryRequest.ParentId,
            Name = createCategoryRequest.Name,
            ImageUrl = createCategoryRequest.ImageUrl,
        };

        public UpdateCategoryCommand Map(Guid categoryId, UpdateCategoryRequest updateCategoryRequest)
        => new UpdateCategoryCommand
        {
            Id = categoryId,
            ParentId = updateCategoryRequest.ParentId,
            Name = updateCategoryRequest.Name,
            ImageUrl = updateCategoryRequest.ImageUrl
        };

        public CategoryViewModel Map(Category category)
        => new CategoryViewModel
        {
            CategoryId = category.Id,
            ParentId = category.ParentCategory?.Id,
            Name = category.Name,
            ImageUrl = category.ImageUrl
        };
    }
}
