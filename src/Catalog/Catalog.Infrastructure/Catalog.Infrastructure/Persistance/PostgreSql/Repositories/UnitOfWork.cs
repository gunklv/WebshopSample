using Catalog.Application.Shared;
using Catalog.Application.Shared.Mappers.Abstractions;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.ItemAggregate;
using Catalog.Domain.Shared.Repositories;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Abstractions;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Mappers;
using Catalog.Infrastructure.Persistance.PostgreSql.Repositories.Utils;
using MediatR;
using Npgsql;
using System.Data;

namespace Catalog.Infrastructure.Persistance.PostgreSql.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly NpgsqlConnection _npgsqlConnection;
        private readonly IChangeTracker _changeTracker;
        private readonly IMediator _mediator;
        private IDbTransaction _dbTransaction;

        public readonly IEnumerable<IIntegrationEventMapper> _integrationEventMappers;

        private IIntegrationEventOutboxRepository _integrationEventOutboxRepository;

        public IItemRepository ItemRepository { get; private set; }
        public IRepository<Category> CategoryRepository { get; private set; }

        public UnitOfWork(NpgsqlConnection npgsqlConnection, IChangeTracker changeTracker, IMediator mediator, IEnumerable<IIntegrationEventMapper> integrationEventMappers)
        {
            _npgsqlConnection = npgsqlConnection;
            _changeTracker = changeTracker;
            _mediator = mediator;
            _integrationEventMappers = integrationEventMappers;
        }

        public async Task BeginAsync()
        {
            await _npgsqlConnection.OpenAsync();

            _dbTransaction = await _npgsqlConnection.BeginTransactionAsync();
            CategoryRepository = new CategoryRepository(_npgsqlConnection, _changeTracker, _dbTransaction, new CategoryMapper());
            ItemRepository = new ItemRepository(_npgsqlConnection, _changeTracker, _dbTransaction, CategoryRepository, new ItemMapper());
            _integrationEventOutboxRepository = new IntegrationEventOutboxRepository(_npgsqlConnection, _dbTransaction);
        }

        public async Task CommitAsync()
        {
            try
            {
                await PublishDomainEventsAsync();
                await PublishIntegrationEventsAsync();

                _dbTransaction.Commit();

                await _npgsqlConnection.CloseAsync();
            }
            catch (Exception)
            {
                _dbTransaction.Rollback();
            }
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
        }

        public void Dispose()
        {
            _npgsqlConnection.Dispose();

            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
        }

        private async Task PublishDomainEventsAsync()
        {
            var domainEvents = _changeTracker.GetChangedAggregateRoots().SelectMany(x => x.DomainEvents).ToList();

            while (domainEvents.Any())
            {
                foreach (var domainEvent in domainEvents)
                {
                    await _mediator.Publish(domainEvent);
                }

                // Filtering out those events which were published, but if there were added any
                // new events while the previous iteration then they have to be published too
                domainEvents = _changeTracker.GetChangedAggregateRoots().SelectMany(x => x.DomainEvents).Where(x => !domainEvents.Contains(x)).ToList();
            }
        }

        private async Task PublishIntegrationEventsAsync()
        {
            var domainEvents = _changeTracker.GetChangedAggregateRoots().SelectMany(x => x.DomainEvents);

            foreach (var domainEvent in domainEvents)
            {
                foreach (var integrationEventMapper in _integrationEventMappers)
                {
                    var integrationEvent = integrationEventMapper.MapDomainEvent(domainEvent);

                    if (integrationEvent != null)
                        await _integrationEventOutboxRepository.InsertAsync(integrationEvent);
                }
            }
        }
    }
}
