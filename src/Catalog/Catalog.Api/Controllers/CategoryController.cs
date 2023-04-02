using Catalog.Api.Attributes;
using Catalog.Api.Exceptions;
using Catalog.Api.Mappers.Abstractions;
using Catalog.Api.Models.Category.Requests;
using Catalog.Api.Utilities.Hateoas.Attributes;
using Catalog.Application.Category.Commands.DeleteCategory;
using Catalog.Application.Category.Queries.GetCategory;
using Catalog.Application.Category.Queries.ListCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryMapper _categoryMapper;
        private readonly IMediator _mediator;

        public CategoryController(ICategoryMapper categoryMapper, IMediator mediator)
        {
            _categoryMapper = categoryMapper;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("", Name = "CreateCategory")]
        [Link(nameof(GetCategoryAsync), "categoryId")]
        [Link(nameof(UpdateCategoryAsync), "categoryId")]
        [Link(nameof(DeleteCategoryAsync), "categoryId")]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidRequestException(ModelState);
            }

            var createCategoryCommand = _categoryMapper.Map(createCategoryRequest);
            var category = await _mediator.Send(createCategoryCommand);

            var categoryViewModel = _categoryMapper.Map(category);

            return StatusCode(StatusCodes.Status201Created, categoryViewModel);
        }

        [HttpGet]
        [Route("{categoryId}", Name = "GetCategory")]
        [Link(nameof(GetCategoryAsync), "categoryId")]
        [Link(nameof(UpdateCategoryAsync), "categoryId")]
        [Link(nameof(DeleteCategoryAsync), "categoryId")]
        public async Task<IActionResult> GetCategoryAsync([FromRoute][NotDefault] Guid categoryId)
        {
            var category = await _mediator.Send(new GetCategoryQuery { Id = categoryId });
            var categoryViewModel = _categoryMapper.Map(category);

            return Ok(categoryViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ListCategoriesAsync()
        {
            var categories = await _mediator.Send(new ListCategoriesQuery());
            var categoryViewModelList = categories.Select(category => _categoryMapper.Map(category));

            return Ok(categoryViewModelList);
        }

        [HttpPut]
        [Route("{categoryId}", Name = "UpdateCategory")]
        [Link(nameof(GetCategoryAsync), "categoryId")]
        [Link(nameof(UpdateCategoryAsync), "categoryId")]
        [Link(nameof(DeleteCategoryAsync), "categoryId")]
        public async Task<IActionResult> UpdateCategoryAsync([FromRoute][NotDefault] Guid categoryId, [FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidRequestException(ModelState);
            }

            var updateCategoryCommand = _categoryMapper.Map(categoryId, updateCategoryRequest);
            var category = await _mediator.Send(updateCategoryCommand);

            var categoryViewModel = _categoryMapper.Map(category);

            return Ok(categoryViewModel);
        }

        [HttpDelete]
        [Route("{categoryId}", Name="DeleteCategory")]
        [Link(nameof(CreateCategoryAsync), "")]
        public async Task<IActionResult> DeleteCategoryAsync([FromRoute][NotDefault] Guid categoryId)
        {
            await _mediator.Send(new DeleteCategoryCommand { Id = categoryId });

            return Ok(new { });
        }
    }
}