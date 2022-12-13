namespace Post.Command.Application.Factories;

public sealed class PostAggregateFactory : IPostAggregateFactory<PostAggregate>
{
    public PostAggregate Create(Guid id, string author, string message) => new (id, author, message);
}