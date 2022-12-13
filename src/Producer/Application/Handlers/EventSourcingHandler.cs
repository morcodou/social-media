namespace Post.Command.Application.Handlers;

public class EventSourcingHandler<TAggregate> : IEventSourcingHandler<TAggregate>
    where TAggregate : AggregateRoot, new()
{
    private readonly IEventStore _eventStore;
    private readonly IEventProducer _eventProducer;

    public EventSourcingHandler(IEventStore eventStore, IEventProducer eventProducer)
    {
        _eventStore = eventStore;
        _eventProducer = eventProducer;
    }

    public async Task<TAggregate> GetByIdAsync(Guid aggregateId)
    {
        TAggregate aggregate = new();
        var events = await _eventStore.GetEventsAsync(aggregateId);
        if (events.IsNullOrEmpty()) return aggregate;

        aggregate.ReplayEvents(events);
        aggregate.Version = events.Max(x => x.Version);

        return aggregate;
    }

    public async Task RepublishEventsAsync()
    {
        var aggregateIds = await _eventStore.GetAggragateIdsAsync();
        if (aggregateIds.IsNullOrEmpty()) return;
        foreach (var aggregateId in aggregateIds)
        {
            var aggregate = await GetByIdAsync(aggregateId);
            if (aggregate == null) return;
            var events = await _eventStore.GetEventsAsync(aggregateId);
            foreach (var @event in events)
            {
                var topic = $"{Environment.GetEnvironmentVariable("KAFKA_TOPIC")}";
                await _eventProducer.ProduceAsync(topic, @event);
            }
        }
    }

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        var uncommittedChanges = aggregate.GetUncommittedChanges();
        await _eventStore.SaveEventsAsync(aggregate.Id, uncommittedChanges, aggregate.Version);
        aggregate.MarkChangesAsCommitted();
    }
}