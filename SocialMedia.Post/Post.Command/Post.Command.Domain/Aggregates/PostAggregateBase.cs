using CQRS.Core.Domain;

namespace Post.Command.Domain.Aggregates
{
    public abstract class PostAggregateBase : AggregateRoot
    {
        public abstract void AddComment(string comment, string username);

        public abstract void DeletePost(string username);

        public abstract void EditComment(Guid commentId, string comment, string username);

        public abstract void EditMessage(string message);

        public abstract void LikePost();

        public abstract void RemoveComment(Guid commentId, string username);
    }
}