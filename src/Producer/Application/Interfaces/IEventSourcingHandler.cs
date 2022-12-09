namespace Post.Command.Application.Interfaces;

public interface IEventSourcingHandler<T>
{
    Task SaveAsync(AggregateRoot aggregate);
    Task<T> GetByIdAsync(Guid aggregateId);
    Task RepublishEventsAsync();
}