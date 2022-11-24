using CQRS.Core.Queries;
using Post.Query.Domain.Entities;
using Post.Query.Infrastructure.Dispatchers;

namespace Post.Query.Infrastructure.Test.Dispatchers
{
    public class QueryDispatcherTest
    {
        private class FakeQuery : BaseQuery { }

        private readonly QueryDispatcher _sut;
        private readonly Fixture _fixture;

        public QueryDispatcherTest()
        {
            _sut = new();

            _fixture = new();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void RegisterHandler_GivenNewType_ShouldRegisterHandler()
        {
            // Arrange
            var posts = _fixture.CreateMany<PostEntity>().ToList();
            var handler = (FakeQuery query) => Task.FromResult(posts);

            // Act
            var exception = Record.Exception(() => _sut.RegisterHandler<FakeQuery>(handler));

            // Assert
            exception.Should().BeNull();
        }

        [Fact]
        public void RegisterHandler_GivenRegisteredType_ShouldTrhowIndexOutOfRangeException()
        {
            // Arrange
            var posts = _fixture.CreateMany<PostEntity>().ToList();
            var handler = (FakeQuery query) => Task.FromResult(posts);
            var otherPosts = _fixture.CreateMany<PostEntity>().ToList();
            var otherHandler = (FakeQuery query) => Task.FromResult(otherPosts);

            // Act
            _sut.RegisterHandler<FakeQuery>(handler);
            var registerTwiceAction = () => _sut.RegisterHandler<FakeQuery>(otherHandler);

            // Assert
            registerTwiceAction
                .Should()
                .Throw<IndexOutOfRangeException>()
                .WithMessage("You cannot register the same query twice!");
        }

        [Fact]
        public async Task SendAsync_GivenRegisterType_ShouldInvokeHandler()
        {
            // Arrange
            var fakeQuery = _fixture.Create<FakeQuery>();
            var posts = _fixture.CreateMany<PostEntity>().ToList();
            var handler = (FakeQuery query) => Task.FromResult(posts);

            // Act
            _sut.RegisterHandler<FakeQuery>(handler);
            var result = await _sut.SendAsync(fakeQuery);

            // Assert
            result.Should().BeEquivalentTo(posts);
        }

        [Fact]
        public async Task SendAsync_GivenUnregisterType_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fakeQuery = _fixture.Create<FakeQuery>();

            // Act
            var sendAsync = async () => await _sut.SendAsync(fakeQuery);

            // Assert
            await sendAsync
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithMessage("No query handler was register (Parameter 'handler')");
        }
    }
}