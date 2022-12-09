namespace Post.Command.Application.Interfaces;

public interface IPostAggregateFactory<TPostAggregate> where TPostAggregate : PostAggregateBase
{
    TPostAggregate Create(Guid id, string author, string message);
}