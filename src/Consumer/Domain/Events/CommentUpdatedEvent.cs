namespace Post.Query.Domain.Events;

[ExcludeFromCodeCoverage]
public sealed class CommentUpdatedEvent : BaseEvent
{
    public Guid CommentId { get; set; }
    public string Comment { get; set; } = null!;
    public string Username { get; set; } = null!;
    public DateTime EditDate { get; set; }
    public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent)) { }
}