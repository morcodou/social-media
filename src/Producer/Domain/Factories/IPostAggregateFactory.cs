namespace Post.Command.Domain.Factories;

public interface IPostAggregateFactory<TPostAggregate> where TPostAggregate : PostAggregateBase
{
    TPostAggregate Create(Guid id, string author, string message);
}