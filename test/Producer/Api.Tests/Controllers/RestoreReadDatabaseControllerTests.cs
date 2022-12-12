namespace Post.Command.Api.Controllers;

public class RestoreReadDatabaseControllerTests : ControllerFixtureBase<RestoreReadDatabaseController>
{
    private readonly RestoreReadDatabaseController _sut;
    private const string _METHODENAME = nameof(RestoreReadDatabaseController.RestoreReadDatabaseAsync);
    public RestoreReadDatabaseControllerTests()
    {
        _sut = new(_logger, _dispatcher);
    }

    [Fact]
    public void RestoreReadDatabaseController_ShouldBe_Decorated_With_ApiControllerAttribute() =>
                Controller_ShouldBe_Decorated_With_ApiControllerAttribute();

    [Fact]
    public void RestoreReadDatabaseController_ShouldBe_Decorated_With_RouteAttribute() =>
                Controller_ShouldBe_Decorated_With_RouteAttribute();

    [Fact]
    public void RestoreReadDatabaseAsync_ShouldBe_Decorated_With_HttpAttribute() =>
                WithHttpPostAttribute(_METHODENAME);

    [Fact]
    public async Task RestoreReadDatabaseAsync_ShouldReturnsCreatedResponse()
    {
        // Arrange
        // Act
        var result = await _sut.RestoreReadDatabaseAsync() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status201Created);
        var response = result!.Value as BaseResponse;
        response!.Message.Should().Be("Restore read database request completed successfully!");
        Mock.Get(_dispatcher)
            .Verify(x => x.SendAsync(It.IsAny<RestoreReadDatabaseCommand>()), Times.Once);
    }

    [Fact]
    public async Task RestoreReadDatabaseAsync_GivenUnhandleException_ShouldReturnsInternalServerError()
    {
        // Arrange
        RestoreReadDatabaseCommand command = null!;
        var message = "An error occurred while processing request to restore read database!";
        var action = () => _sut.RestoreReadDatabaseAsync();

        // Act
        await UnhandleException_ShouldReturnsInternalServerError(action, command, message);
    }
}