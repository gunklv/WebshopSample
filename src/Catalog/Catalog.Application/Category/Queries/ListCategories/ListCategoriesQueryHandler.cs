using Catalog.Application.Shared;
using MediatR;

namespace Catalog.Application.Category.Queries.ListCategories
{
    internal class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, IReadOnlyCollection<Domain.CategoryAggregate.Category>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListCategoriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyCollection<Domain.CategoryAggregate.Category>> Handle(ListCategoriesQuery listCategoriesQuery, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginAsync();
            try
            {
                return await _unitOfWork.CategoryRepository.GetAllAsync();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
