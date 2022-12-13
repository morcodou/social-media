namespace Post.Command.Domain.Messages;

[ExcludeFromCodeCoverage]
public abstract class Message
{
    public Guid Id { get; set; }
}