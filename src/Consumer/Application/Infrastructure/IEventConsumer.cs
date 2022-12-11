namespace Post.Query.Application.Infrastructure;

public interface IEventConsumer
{
    void Consume(string topic);
}