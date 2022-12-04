using CQRS.Core.Domain;

namespace CQRS.Core.Test.Domain
{
    public class NoApplyPostAggregate : AggregateRoot { }

    public class InvalidAggregateRootTest
    {
        private readonly NoApplyPostAggregate _sut = new NoApplyPostAggregate();
        private readonly Fixture _fixture = new();

        [Fact]
        public void ApplyChanges_GivenEven_ShouldThrowsArgumentNullException()
        {
            // Arrange
            var @event = _fixture.Create<FakeEvent>();
            var isNew = _fixture.Create<bool>();

            // Act
            var acttionApplyChanges = () => _sut.ApplyChanges(@event, isNew);

            // Assert
            ShouldThrowsArgumentNullExceptions(acttionApplyChanges);
        }

        [Fact]
        public void ReplayEvents_GivenEvents_ShouldThrowsArgumentNullException()
        {
            // Arrange
            var events = _fixture.CreateMany<FakeEvent>();

            // Act
            var acttionReplayEvents = () => _sut.ReplayEvents(events);

            // Assert
            ShouldThrowsArgumentNullExceptions(acttionReplayEvents);
        }

        private void ShouldThrowsArgumentNullExceptions(Action action) =>
                action
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage($"The Apply method was not found in the aggregate for {typeof(FakeEvent).Name} (Parameter 'method')");
    }
}