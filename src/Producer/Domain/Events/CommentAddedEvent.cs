
namespace Post.Command.Domain.Events;

[ExcludeFromCodeCoverage]
public sealed class CommentAddedEvent : BaseEvent
{
    public Guid CommentId { get; set; }
    public string Comment { get; set; } = null!;
    public string Username { get; set; } = null!;
    public DateTime CommentDate { get; set; }
    public CommentAddedEvent() : base(nameof(CommentAddedEvent)) { }
}