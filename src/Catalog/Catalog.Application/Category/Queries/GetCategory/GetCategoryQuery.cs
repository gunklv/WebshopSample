using MediatR;

namespace Catalog.Application.Category.Queries.GetCategory
{
    public class GetCategoryQuery : IRequest<Domain.CategoryAggregate.Category>
    {
        public Guid Id { get; set; }
    }
}
