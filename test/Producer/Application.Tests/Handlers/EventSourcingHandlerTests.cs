using Post.Command.Application.Handlers;
using Post.Command.Application.Infrastructure;

namespace Post.Command.Infrastructure.Handlers;

public class EventSourcingHandlerTests
{
    private class FakePostAggregate : AggregateRoot
    {
        public FakePostAggregate()
        {
        }
    }

    // TODO

    private readonly EventSourcingHandler<FakePostAggregate> _sut;
    private readonly IEventStore _eventStore;
    private readonly IEventProducer _eventProducer;

    public EventSourcingHandlerTests()
    {
        _eventStore = Mock.Of<IEventStore>();
        _eventProducer = Mock.Of<IEventProducer>();
        _sut = new(_eventStore, _eventProducer);
    }
}