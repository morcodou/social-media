using Post.Command.Application.Exceptions;
using Post.Command.Infrastructure.Models;

namespace Post.Command.Infrastructure.Stores;

public class EventStore : IEventStore
{
    private readonly IEventStoreRepository<EventModel> _eventStoreRepository;
    private readonly IEventProducer _eventProducer;

    public EventStore(IEventStoreRepository<EventModel> eventStoreRepository, IEventProducer eventProducer)
    {
        _eventStoreRepository = eventStoreRepository;
        _eventProducer = eventProducer;
    }

    public async Task<List<Guid>> GetAggragateIdsAsync()
    {
        var eventStream = await _eventStoreRepository.FindAllAsync();
        if (eventStream == null || !eventStream.Any())
        {
            throw new ArgumentNullException(nameof(eventStream), "Could not retrieve eventStream from the eventStore!");
        }

        return eventStream.Select(x => x.AggregateIdentifier).Distinct().ToList();
    }

    public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
        if (eventStream == null || !eventStream.Any())
        {
            throw new AggregateNotFoundException("Incorrect post Id provided");
        }

        return eventStream.OrderBy(x => x.Version)
                          .Select(x => x.EventData)
                          .ToList();
    }

    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
        if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
        {
            throw new ConcurrencyException();
        }

        var version = expectedVersion;
        foreach (var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;
            var eventModel = new EventModel()
            {
                TimeStamp = DateTime.Now,
                AggregateIdentifier = aggregateId,
                AggregateType = nameof(PostAggregate),
                Version = version,
                EventType = eventType,
                EventData = @event
            };

            await _eventStoreRepository.SaveAsync(eventModel);

            var topic = $"{Environment.GetEnvironmentVariable("KAFKA_TOPIC")}";
            await _eventProducer.ProduceAsync(topic, @event);
        }
    }
}