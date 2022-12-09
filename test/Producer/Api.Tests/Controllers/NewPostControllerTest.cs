namespace Post.Command.Api.Test.Controllers;

public class NewPostControllerTest : ControllerFixtureBase<NewPostController>
{
    private readonly NewPostController _sut;
    private const string _METHODENAME = nameof(NewPostController.NewPostAsync);
    public NewPostControllerTest()
    {
        _sut = new(_logger, _dispatcher);
    }

    [Fact]
    public void NewPostController_ShouldBe_Decorated_With_ApiControllerAttribute() =>
                Controller_ShouldBe_Decorated_With_ApiControllerAttribute();

    [Fact]
    public void NewPostController_ShouldBe_Decorated_With_RouteAttribute() =>
                Controller_ShouldBe_Decorated_With_RouteAttribute();

    [Fact]
    public void NewPostAsync_ShouldBe_Decorated_With_HttpAttribute() =>
                WithHttpPostAttribute(_METHODENAME);

    [Fact]
    public async Task NewPostAsync_GivenNewPostCommand_ShouldReturnsCreatedResponse()
    {
        // Arrange
        var command = _fixture.Create<NewPostCommand>();

        // Act
        var result = await _sut.NewPostAsync(command) as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        var response = result!.Value as PostResponse;
        result!.StatusCode.Should().Be(StatusCodes.Status201Created);
        response!.Id.Should().NotBeEmpty();
        response!.Message.Should().Be("New post creation request completed successfully!");
        Mock.Get(_dispatcher)
            .Verify(x => x.SendAsync(command), Times.Once);
    }

    [Fact]
    public async Task NewPostAsync_GivenUnhandleException_ShouldReturnsInternalServerError()
    {
        // Arrange
        var command = _fixture.Create<NewPostCommand>();
        var message = "An error occurred while processing request to create a new post!";
        var action = () => _sut.NewPostAsync(command);

        // Act
        await UnhandleException_ShouldReturnsInternalServerError(action, command, message);
    }

    [Fact]
    public async Task NewPostAsync_GivenInvalidOperationException_ShouldReturnsBadRequest()
    {
        // Arrange
        var command = _fixture.Create<NewPostCommand>();
        var action = () => _sut.NewPostAsync(command);

        // Act
        await InvalidOperationException_ShouldReturnsBadRequest(action, command);
    }
}