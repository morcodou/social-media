using Post.Command.Domain.Factories;
using Post.Common.Events;

namespace Post.Command.Domain.Test.Factories
{
    public class PostAggregateFactoryTest
    {
        private readonly PostAggregateFactory _sut = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public void Create_ShouldReturnsPostAggregate()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            var author = _fixture.Create<string>();
            var message = _fixture.Create<string>();

            // Act
            var result = _sut.Create(id, author, message);

            // Assert
            result.Id.Should().Be(id);
            result.Active.Should().BeTrue();
            var @event = result.GetUncommittedChanges().OfType<PostCreatedEvent>().First();
            @event.Id.Should().Be(id);
            @event.Author.Should().Be(author);
            @event.Message.Should().Be(message);
        }
    }
}