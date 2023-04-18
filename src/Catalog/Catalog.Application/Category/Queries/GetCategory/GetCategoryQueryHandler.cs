using Catalog.Application.Shared;
using Catalog.Application.Shared.Exceptions;
using MediatR;

namespace Catalog.Application.Category.Queries.GetCategory
{
    internal class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Domain.CategoryAggregate.Category>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Domain.CategoryAggregate.Category> Handle(GetCategoryQuery getCategoryQuery, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginAsync();
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(getCategoryQuery.Id);
                if (category == null)
                    throw new CategoryNotFoundException(
                        $"Could not found Category with id: {getCategoryQuery.Id}.");

                return category;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
