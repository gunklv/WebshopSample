using MediatR;

namespace Catalog.Application.Category.Queries.GetCategory
{
    public class GetCategoryQuery : IRequest<Domain.Aggregates.Category>
    {
        public Guid Id { get; set; }
    }
}
