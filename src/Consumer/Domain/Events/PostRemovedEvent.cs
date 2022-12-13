namespace Post.Query.Domain.Events;

[ExcludeFromCodeCoverage]
public sealed class PostRemovedEvent : BaseEvent
{
    public PostRemovedEvent() : base(nameof(PostRemovedEvent)) { }
}