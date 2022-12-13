namespace Post.Command.Api.Controllers;

public class EditMessageControllerTests : ControllerFixtureBase<EditMessageController>
{
    private readonly EditMessageController _sut;
    private const string _METHODENAME = nameof(EditMessageController.EditMessageAsync);
    private const string _ATTRIBUTETEMPLATE = "{id}";

    public EditMessageControllerTests() => _sut = new(_logger, _dispatcher);

    [Fact]
    public void EditMessageController_ShouldBe_Decorated_With_ApiControllerAttribute() =>
                Controller_ShouldBe_Decorated_With_ApiControllerAttribute();

    [Fact]
    public void EditMessageController_ShouldBe_Decorated_With_RouteAttribute() =>
                Controller_ShouldBe_Decorated_With_RouteAttribute();

    [Fact]
    public void EditMessageAsync_ShouldBe_Decorated_With_HttpGetAttribute() =>
                WithHttpPutAttribute(_METHODENAME, _ATTRIBUTETEMPLATE);

    [Fact]
    public async Task EditMessageAsync_GivenIdAndEditMessageCommand_ShouldReturnsOkResponse()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var command = _fixture.Create<EditMessageCommand>();
        var response = new PostResponse()
        {
            Id = id,
            Message = "Edit message request completed successfully!"
        };

        // Act
        var result = await _sut.EditMessageAsync(id, command) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(response);
        command.Id.Should().Be(id);
        Mock.Get(_dispatcher)
            .Verify(x => x.SendAsync(command), Times.Once);
    }

    [Fact]
    public async Task EditMessageAsync_GivenUnhandleException_ShouldReturnsInternalServerError()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var command = _fixture.Create<EditMessageCommand>();
        var action = () => _sut.EditMessageAsync(id, command);
        var message = "An error occurred while processing request to edit the message!";

        // Act
        await UnhandleException_ShouldReturnsInternalServerError(action, command, message);
    }

    [Fact]
    public async Task EditMessageAsync_GivenInvalidOperationException_ShouldReturnsBadRequest()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var command = _fixture.Create<EditMessageCommand>();
        var action = () => _sut.EditMessageAsync(id, command);

        // Act
        await InvalidOperationException_ShouldReturnsBadRequest(action, command);
    }

    [Fact]
    public async Task EditMessageAsync_GivenAggregateNotFoundException_ShouldReturnsBadRequest()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var command = _fixture.Create<EditMessageCommand>();
        var action = () => _sut.EditMessageAsync(id, command);

        // Act
        await AggregateNotFoundException_ShouldReturnsBadRequest(action, command);
    }
}