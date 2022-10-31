using Post.Command.Domain.Aggregates;

namespace Post.Command.Domain.Factories
{
    public class PostAggregateFactory : IPostAggregateFactory<PostAggregate>
    {
        public PostAggregate Create(Guid id, string author, string message) => new (id, author, message);
    }
}