using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Post.Command.Domain.Aggregates;

namespace Post.Command.Infrastructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;

        public EventStore(IEventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<List<BaseEvent>> GetEventsAsync(Guid aggragateId)
        {
            var eventStream = await _eventStoreRepository.FindByAggregateId(aggragateId);
            if (eventStream == null || !eventStream.Any())
            {
                throw new AggragateNotFoundException("Incorrect post Id provided");
            }

            return eventStream.OrderBy(x => x.Version)
                              .Select(x => x.EventData)
                              .ToList();
        }

        public async Task SaveEventsAsync(Guid aggragateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = await _eventStoreRepository.FindByAggregateId(aggragateId);
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
                    AggregateIdentifier = aggragateId,
                    AggregateType = nameof(PostAggregate),
                    Version = version,
                    EventType = eventType,
                    EventData = @event
                };

                await _eventStoreRepository.SaveAsync(eventModel);
            }
        }
    }
}