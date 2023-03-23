using Catalog.Api.Attributes;
using Catalog.Api.Exceptions;
using Catalog.Api.Mappers.Abstractions;
using Catalog.Api.Models.Category.Requests;
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
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidRequestException(ModelState);
            }

            var createCategoryCommand = _categoryMapper.Map(createCategoryRequest);
            await _mediator.Send(createCategoryCommand);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        [Route("{categoryId}")]
        public async Task<IActionResult> GetAsync([FromRoute][NotDefault] Guid categoryId)
        {
            var category = await _mediator.Send(new GetCategoryQuery { Id = categoryId });
            var categoryDto = _categoryMapper.Map(category);

            return Ok(categoryDto);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var categories = await _mediator.Send(new ListCategoriesQuery());
            var categoryDtoList = categories.Select(category => _categoryMapper.Map(category));

            return Ok(categoryDtoList);
        }

        [HttpPut]
        [Route("{categoryId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute][NotDefault] Guid categoryId, [FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidRequestException(ModelState);
            }

            var updateCategoryCommand = _categoryMapper.Map(categoryId, updateCategoryRequest);
            await _mediator.Send(updateCategoryCommand);

            return Ok();
        }

        [HttpDelete]
        [Route("{categoryId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute][NotDefault] Guid categoryId)
        {
            await _mediator.Send(new DeleteCategoryCommand { Id = categoryId });

            return Ok();
        }
    }
}