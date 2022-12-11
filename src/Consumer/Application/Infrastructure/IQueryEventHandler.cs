namespace Post.Query.Application.Infrastructure;

public interface IQueryEventHandler
{
    Task On(PostCreatedEvent @event);

    Task On(PostLikedEvent @event);

    Task On(PostRemovedEvent @event);

    Task On(CommentAddedEvent @event);

    Task On(CommentUpdatedEvent @event);

    Task On(CommentRemovedEvent @event);

    Task On(MessageUpdatedEvent @event);
}