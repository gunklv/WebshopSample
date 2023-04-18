using MediatR;

namespace Catalog.Application.Category.Queries.ListCategories
{
    public class ListCategoriesQuery : IRequest<IReadOnlyCollection<Domain.CategoryAggregate.Category>>
    {
    }
}
