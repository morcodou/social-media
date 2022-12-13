
namespace Post.Command.Domain.Events;

[ExcludeFromCodeCoverage]
public sealed class CommentRemovedEvent : BaseEvent
{
    public Guid CommentId { get; set; }
    public CommentRemovedEvent() : base(nameof(CommentRemovedEvent)) { }
}