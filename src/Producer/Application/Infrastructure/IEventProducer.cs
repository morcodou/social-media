namespace Post.Command.Application.Infrastructure;

public interface IEventProducer
{
    Task<EventPersistenceStatus> ProduceAsync<T>(string topic, T @event) where T : BaseEvent;
}