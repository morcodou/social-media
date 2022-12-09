using Post.Command.Application.Dispatchers;

namespace Post.Command.Infrastructure.Test.Dispatchers;

public class CommandDispatcherTest
{
    private class FakeCommand : BaseCommand
    {
        public bool WasHandle { get; set; }
    }

    private readonly CommandDispatcher _sut;
    private readonly Fixture _fixture;

    public CommandDispatcherTest()
    {
        _sut = new();
        _fixture = new();
    }

    [Fact]
    public async Task RegisterHandler_GivenUnregisterHandler_ShouldRegisterHandler()
    {
        // Arrange
        Func<FakeCommand, Task> handler = async (command) =>
        {
            await Task.Delay(0);
            command.WasHandle = true;
        };
        var fakeCommand = _fixture.Create<FakeCommand>();

        // Act
        _sut.RegisterHandler<FakeCommand>(handler);
        await _sut.SendAsync(fakeCommand);

        // Assert
        fakeCommand.Should().NotBeNull();
        fakeCommand.WasHandle.Should().BeTrue();
    }

    [Fact]
    public void RegisterHandler_GivenRegisteredHandler_ShouldThrowsIndexOutOfRangeException()
    {
        // Arrange
        Func<FakeCommand, Task> handler = async (command) =>
        {
            await Task.Delay(0);
            command.WasHandle = true;
        };
        var fakeCommand = _fixture.Create<FakeCommand>();

        // Act
        _sut.RegisterHandler<FakeCommand>(handler);
        var exception = Assert.Throws<IndexOutOfRangeException>(() => _sut.RegisterHandler<FakeCommand>(handler));

        // Assert
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task SendAsync_GivenRegisterCommandType_ShouldCallTheHandler()
    {
        // Arrange
        Func<FakeCommand, Task> handler = async (command) =>
        {
            await Task.Delay(0);
            command.WasHandle = true;
        };
        var fakeCommand = _fixture.Create<FakeCommand>();

        // Act
        _sut.RegisterHandler<FakeCommand>(handler);
        await _sut.SendAsync(fakeCommand);

        // Assert
        fakeCommand.Should().NotBeNull();
        fakeCommand.WasHandle.Should().BeTrue();
    }

    [Fact]
    public void SendAsync_GivenUnregisteredCommandType_ShouldThrowsArgumentNullException()
    {
        // Arrange
        var fakeCommand = _fixture.Create<FakeCommand>();

        // Act
        var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _sut.SendAsync(fakeCommand));

        // Assert
        exception.Should().NotBeNull();
    }
}