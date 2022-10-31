using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;

namespace Post.Command.Infrastructure.Handlers
{
    public class EventSourcingHandler<TAggregate> : IEventSourcingHandler<TAggregate>
        where TAggregate : AggregateRoot, new()
    {
        private readonly IEventStore _eventStore;

        public EventSourcingHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<TAggregate> GetByIdAsync(Guid aggregateId)
        {
            TAggregate aggregate = new();
            var events = await _eventStore.GetEventsAsync(aggregateId);
            if (events == null || !events.Any()) return aggregate;

            aggregate.ReplayEvents(events);
            aggregate.Version = events.Max(x => x.Version);

            return aggregate;
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            var uncommittedChanges = aggregate.GetUncommittedChanges();
            await _eventStore.SaveEventsAsync(aggregate.Id, uncommittedChanges, aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}