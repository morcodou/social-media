namespace Post.Query.Domain.Messages;

[ExcludeFromCodeCoverage]
public abstract class Message
{
    public Guid Id { get; set; }
}