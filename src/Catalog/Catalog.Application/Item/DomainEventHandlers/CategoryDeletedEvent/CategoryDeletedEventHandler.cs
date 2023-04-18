using Catalog.Application.Shared;
using MediatR;

namespace Catalog.Application.Item.DomainEventHandlers.CategoryDeletedEvent
{
    internal class CategoryDeletedEventHandler : INotificationHandler<Domain.CategoryAggregate.Events.CategoryDeletedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryDeletedEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(Domain.CategoryAggregate.Events.CategoryDeletedEvent categoryDeletedEvent, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.ItemRepository.GetAllAsync();
            var itemsAssignedToCategory = items.Where(i => i.CategoryId == categoryDeletedEvent.CategoryId);

            foreach (var item in itemsAssignedToCategory)
            {
                await _unitOfWork.ItemRepository.DeleteAsync(item);
            }
        }
    }
}
