namespace Post.Command.Application.Exceptions;

[ExcludeFromCodeCoverage]
public class AggregateNotFoundException : Exception
{
    public AggregateNotFoundException(string message) : base(message) { }
}