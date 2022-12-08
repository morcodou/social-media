using Post.Command.Domain.Messages;

namespace Post.Command.Domain.Events;

[ExcludeFromCodeCoverage]
public abstract class BaseEvent : Message
{
    public int Version { get; set; }
    public string Type { get; set; }

    protected BaseEvent(string type) => Type = type;
}