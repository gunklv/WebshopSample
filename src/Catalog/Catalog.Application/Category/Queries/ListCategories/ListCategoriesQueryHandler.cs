using Catalog.Domain.Abstractions;
using MediatR;

namespace Catalog.Application.Category.Queries.ListCategories
{
    internal class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, IReadOnlyCollection<Domain.Aggregates.Category>>
    {
        private readonly IRepository<Domain.Aggregates.Category> _categoryRepository;

        public ListCategoriesQueryHandler(IRepository<Domain.Aggregates.Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IReadOnlyCollection<Domain.Aggregates.Category>> Handle(ListCategoriesQuery listCategoriesQuery, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetAllAsync();
        }
    }
}
