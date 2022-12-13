namespace Post.Command.Application.Infrastructure;

public interface IEventStoreRepository<TEventModel>
{
    Task SaveAsync(TEventModel @event);
    Task<List<TEventModel>> FindByAggregateId(Guid aggregateId);
    Task<List<TEventModel>> FindAllAsync();
}