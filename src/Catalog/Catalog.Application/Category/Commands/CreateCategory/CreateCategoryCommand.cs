using MediatR;

namespace Catalog.Application.Category.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<Domain.Aggregates.Category>
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
