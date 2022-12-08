namespace Post.Command.Domain.Events;

[ExcludeFromCodeCoverage]
public sealed class PostLikedEvent : BaseEvent
{
    public PostLikedEvent() : base(nameof(PostLikedEvent)) { }
}