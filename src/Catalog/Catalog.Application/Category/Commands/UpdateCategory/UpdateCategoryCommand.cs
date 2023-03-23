using MediatR;

namespace Catalog.Application.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
