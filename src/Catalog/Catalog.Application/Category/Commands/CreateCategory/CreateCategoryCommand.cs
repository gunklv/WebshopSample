using MediatR;

namespace Catalog.Application.Category.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
