using Post.Query.Domain.Messages;

namespace Post.Query.Domain.Events;

[ExcludeFromCodeCoverage]
public abstract class BaseEvent : Message
{
    public int Version { get; set; }
    public string Type { get; set; }

    protected BaseEvent(string type) => Type = type;
}