using CQRS.Core.Enums;
using CQRS.Core.Events;

namespace CQRS.Core.Producers
{
    public interface IEventProducer
    {
        Task<EventPersistenceStatus> ProduceAsync<T>(string topic, T @event) where T : BaseEvent;
    }
}