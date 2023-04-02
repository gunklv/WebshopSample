using Catalog.Application.Shared.Exceptions;
using Catalog.Domain.Abstractions;
using MediatR;

namespace Catalog.Application.Category.Queries.GetCategory
{
    internal class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Domain.Aggregates.Category>
    {
        private readonly IRepository<Domain.Aggregates.Category> _categoryRepository;

        public GetCategoryQueryHandler(IRepository<Domain.Aggregates.Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Domain.Aggregates.Category> Handle(GetCategoryQuery getCategoryQuery, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(getCategoryQuery.Id);
            if (category == null)
                throw new CategoryNotFoundException(
                    $"Could not found Category with id: {getCategoryQuery.Id}.");

            return category;
        }
    }
}
