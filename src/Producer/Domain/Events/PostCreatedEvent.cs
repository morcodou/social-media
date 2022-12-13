namespace Post.Command.Domain.Events;

[ExcludeFromCodeCoverage]
public sealed class PostCreatedEvent : BaseEvent
{
    public string Author { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime DatePosted { get; set; }

    public PostCreatedEvent() : base(nameof(PostCreatedEvent)) { }
}