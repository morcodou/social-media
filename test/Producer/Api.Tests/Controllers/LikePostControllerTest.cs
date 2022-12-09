namespace Post.Command.Api.Test.Controllers;

public class LikePostControllerTest : ControllerFixtureBase<LikePostController>
{
    private readonly LikePostController _sut;
    private const string _METHODENAME = nameof(LikePostController.LikePostAsync);
    private const string _ATTRIBUTETEMPLATE = "{id}";

    public LikePostControllerTest() => _sut = new(_logger, _dispatcher);

    [Fact]
    public void LikePostController_ShouldBe_Decorated_With_ApiControllerAttribute() =>
                Controller_ShouldBe_Decorated_With_ApiControllerAttribute();

    [Fact]
    public void LikePostController_ShouldBe_Decorated_With_RouteAttribute() =>
                Controller_ShouldBe_Decorated_With_RouteAttribute();

    [Fact]
    public void LikePostAsync_ShouldBe_Decorated_With_HttpGetAttribute() =>
                WithHttpPutAttribute(_METHODENAME, _ATTRIBUTETEMPLATE);

    [Fact]
    public async Task LikePostAsync_GivenId_ShouldReturnsOkResponse()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var response = new PostResponse()
        {
            Id = id,
            Message = "Like post request completed successfully!"
        };

        // Act
        var result = await _sut.LikePostAsync(id) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(response);
        Mock.Get(_dispatcher)
            .Verify(x => x.SendAsync(It.Is<LikePostCommand>(x => x.Id == id)), Times.Once);
    }

    [Fact]
    public async Task LikePostAsync_GivenUnhandleException_ShouldReturnsInternalServerError()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        LikePostCommand command = null!;
        var action = () => _sut.LikePostAsync(id);
        var message = "An error occurred while processing request to like a post!";

        // Act
        await UnhandleException_ShouldReturnsInternalServerError(action, command, message);
    }

    [Fact]
    public async Task LikePostAsync_GivenInvalidOperationException_ShouldReturnsBadRequest()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        LikePostCommand command = null!;
        var action = () => _sut.LikePostAsync(id);

        // Act
        await InvalidOperationException_ShouldReturnsBadRequest(action, command);
    }

    [Fact]
    public async Task LikePostAsync_GivenAggregateNotFoundException_ShouldReturnsBadRequest()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        LikePostCommand command = null!;
        var action = () => _sut.LikePostAsync(id);

        // Act
        await AggregateNotFoundException_ShouldReturnsBadRequest(action, command);
    }
}