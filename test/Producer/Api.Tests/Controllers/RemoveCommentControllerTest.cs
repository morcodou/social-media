namespace Post.Command.Api.Test.Controllers;

public class RemoveCommentControllerTest : ControllerFixtureBase<RemoveCommentController>
{
    private readonly RemoveCommentController _sut;
    private const string _METHODENAME = nameof(RemoveCommentController.RemoveCommentAsync);
    private const string _ATTRIBUTETEMPLATE = "{id}";

    public RemoveCommentControllerTest() => _sut = new(_logger, _dispatcher);

    [Fact]
    public void RemoveCommentController_ShouldBe_Decorated_With_ApiControllerAttribute() =>
                Controller_ShouldBe_Decorated_With_ApiControllerAttribute();

    [Fact]
    public void RemoveCommentController_ShouldBe_Decorated_With_RouteAttribute() =>
                Controller_ShouldBe_Decorated_With_RouteAttribute();

    [Fact]
    public void RemoveCommentAsync_ShouldBe_Decorated_With_HttpGetAttribute() =>
                WithHttpDeleteAttribute(_METHODENAME, _ATTRIBUTETEMPLATE);

    [Fact]
    public async Task RemoveCommentAsync_GivenIdAndRemoveCommentCommand_ShouldReturnsOkResponse()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var command = _fixture.Create<RemoveCommentCommand>();
        var response = new BaseResponse("Remove comment request completed successfully!");

        // Act
        var result = await _sut.RemoveCommentAsync(id, command) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(response);
        command.Id.Should().Be(id);
        Mock.Get(_dispatcher)
            .Verify(x => x.SendAsync(command), Times.Once);
    }

    [Fact]
    public async Task RemoveCommentAsync_GivenUnhandleException_ShouldReturnsInternalServerError()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var command = _fixture.Create<RemoveCommentCommand>();
        var action = () => _sut.RemoveCommentAsync(id, command);
        var message = "An error occurred while processing request to remove comment!";

        // Act
        await UnhandleException_ShouldReturnsInternalServerError(action, command, message);
    }

    [Fact]
    public async Task RemoveCommentAsync_GivenInvalidOperationException_ShouldReturnsBadRequest()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var command = _fixture.Create<RemoveCommentCommand>();
        var action = () => _sut.RemoveCommentAsync(id, command);

        // Act
        await InvalidOperationException_ShouldReturnsBadRequest(action, command);
    }

    [Fact]
    public async Task RemoveCommentAsync_GivenAggregateNotFoundException_ShouldReturnsBadRequest()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var command = _fixture.Create<RemoveCommentCommand>();
        var action = () => _sut.RemoveCommentAsync(id, command);

        // Act
        await AggregateNotFoundException_ShouldReturnsBadRequest(action, command);
    }
}