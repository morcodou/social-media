namespace Post.Command.Api.Test.Controllers;

public class EditCommentControllerTest : ControllerFixtureBase<EditCommentController>
{
    private readonly EditCommentController _sut;
    private const string _METHODENAME = nameof(EditCommentController.EditCommentAsync);
    private const string _ATTRIBUTETEMPLATE = "{id}";

    public EditCommentControllerTest() => _sut = new(_logger, _dispatcher);

    [Fact]
    public void EditCommentController_ShouldBe_Decorated_With_ApiControllerAttribute() =>
                Controller_ShouldBe_Decorated_With_ApiControllerAttribute();

    [Fact]
    public void EditCommentController_ShouldBe_Decorated_With_RouteAttribute() =>
                Controller_ShouldBe_Decorated_With_RouteAttribute();

    [Fact]
    public void EditCommentAsync_ShouldBe_Decorated_With_HttpGetAttribute() =>
                WithHttpPutAttribute(_METHODENAME, _ATTRIBUTETEMPLATE);

    [Fact]
    public async Task EditCommentAsync_GivenIdAndEditCommentCommand_ShouldReturnsOkResponse()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var command = _fixture.Create<EditCommentCommand>();
        var response = new PostResponse()
        {
            Id = id,
            Message = "Edit comment request completed successfully!"
        };

        // Act
        var result = await _sut.EditCommentAsync(id, command) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(response);
        command.Id.Should().Be(id);
        Mock.Get(_dispatcher)
            .Verify(x => x.SendAsync(command), Times.Once);
    }

    [Fact]
    public async Task EditCommentAsync_GivenUnhandleException_ShouldReturnsInternalServerError()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var command = _fixture.Create<EditCommentCommand>();
        var action = () => _sut.EditCommentAsync(id, command);
        var message = "An error occurred while processing request to edit comment!";

        // Act
        await UnhandleException_ShouldReturnsInternalServerError(action, command, message);
    }

    [Fact]
    public async Task EditCommentAsync_GivenInvalidOperationException_ShouldReturnsBadRequest()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var command = _fixture.Create<EditCommentCommand>();
        var action = () => _sut.EditCommentAsync(id, command);

        // Act
        await InvalidOperationException_ShouldReturnsBadRequest(action, command);
    }

    [Fact]
    public async Task EditCommentAsync_GivenAggregateNotFoundException_ShouldReturnsBadRequest()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var command = _fixture.Create<EditCommentCommand>();
        var action = () => _sut.EditCommentAsync(id, command);

        // Act
        await AggregateNotFoundException_ShouldReturnsBadRequest(action, command);
    }
}