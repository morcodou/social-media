using CQRS.Core.Domain;
using CQRS.Core.Events;

namespace CQRS.Core.Test.Domain
{
    public class FakeEvent : BaseEvent
    {
        public FakeEvent() : base(nameof(FakeEvent)) { }
    }

    public class FakePostAggregate : AggregateRoot
    {
        public IReadOnlyList<FakeEvent> Events() => new List<FakeEvent>(_events);
        private readonly List<FakeEvent> _events = new();
        public void Apply(FakeEvent @event) => _events.Add(@event);
    }

    public class AggregateRootTest
    {
        private readonly FakePostAggregate _sut = new FakePostAggregate();
        private readonly Fixture _fixture = new();

        [Fact]
        public void ApplyChanges_GivenNewEven_ShouldInvokeApplyAndAddToUncommittedChanges()
        {
            // Arrange
            var @event = _fixture.Create<FakeEvent>();
            var isNew = true;

            // Act
            _sut.ApplyChanges(@event, isNew);

            // Assert
            _sut.Events().Should().Contain(@event);
            _sut.GetUncommittedChanges().Should().Contain(@event);
        }

        [Fact]
        public void ApplyChanges_GivenNotNewEven_ShouldOnlyInvokeApply()
        {
            // Arrange
            var @event = _fixture.Create<FakeEvent>();
            var isNew = false;

            // Act
            _sut.ApplyChanges(@event, isNew);

            // Assert
            _sut.Events().Should().Contain(@event);
            _sut.GetUncommittedChanges().Should().BeEmpty();
        }

        [Fact]
        public void ReplayEvents_GivenEvents_ShouldInvokeApplyForEachEvent()
        {
            // Arrange
            var events = _fixture.CreateMany<FakeEvent>();

            // Act
            _sut.ReplayEvents(@events);

            // Assert
            _sut.Events().Should().BeEquivalentTo(events);
            _sut.GetUncommittedChanges().Should().BeEmpty();
        }

        [Fact]
        public void MarkChangesAsCommitted_ShouldCommitChanges()
        {
            // Arrange
            var @event = _fixture.Create<FakeEvent>();
            var isNew = true;
            _sut.ApplyChanges(@event, isNew);
            _sut.GetUncommittedChanges().Should().Contain(@event);

            // Act
            _sut.MarkChangesAsCommitted();

            // Assert
            _sut.GetUncommittedChanges().Should().BeEmpty();
        }
    }
}