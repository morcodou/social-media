namespace Post.Command.Domain.Events;

[ExcludeFromCodeCoverage]
public sealed class MessageUpdatedEvent : BaseEvent
{
    public string Message { get; set; } = null!;
    public MessageUpdatedEvent() : base(nameof(MessageUpdatedEvent)) { }
}