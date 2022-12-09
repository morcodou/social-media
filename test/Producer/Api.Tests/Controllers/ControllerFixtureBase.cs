namespace Post.Command.Api.Controllers;

public class ControllerFixtureBase<TController>
{
    protected readonly ICommandDispatcher _dispatcher = Mock.Of<ICommandDispatcher>();
    protected readonly ILogger<TController> _logger = Mock.Of<ILogger<TController>>();
    protected readonly Fixture _fixture = new();

    protected void Controller_ShouldBe_Decorated_With_RouteAttribute()
    {
        var attributeFunc = (RouteAttribute attribute) => attribute != null && attribute.Template == "api/v1/[controller]";

        // Act
        var decorated = AttributeExtensions.IsDecoratedWithAttribute<TController, RouteAttribute>(attributeFunc);

        // Assert
        decorated.Should().BeTrue();
    }

    protected void Controller_ShouldBe_Decorated_With_ApiControllerAttribute()
    {
        // Arrange
        var attributeFunc = (ApiControllerAttribute attribute) => attribute != null;

        // Act
        var decorated = AttributeExtensions.IsDecoratedWithAttribute<TController, ApiControllerAttribute>(attributeFunc);

        // Assert
        decorated.Should().BeTrue();
    }

    protected async Task UnhandleException_ShouldReturnsInternalServerError<TCommand>(
        Func<Task<ActionResult>> action,
        TCommand command,
        string message)
        where TCommand : BaseCommand
    {
        // Arrange
        var exception = new Exception("unhandle_exception");
        Mock.Get(_dispatcher)
             .Setup(x => x.SendAsync(It.IsAny<BaseCommand>()))
             .ThrowsAsync(exception);

        // Act
        var result = await action() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        var response = result!.Value as BaseResponse;
        response!.Message.Should().Be(message);
        Mock.Get(_dispatcher)
            .Verify(x => x.SendAsync(It.Is<TCommand>(x => command == null ? x != null : x == command)), Times.AtLeastOnce);
        _logger.VerifyLoggerError(message);
    }

    protected async Task InvalidOperationException_ShouldReturnsBadRequest<TCommand>(
        Func<Task<ActionResult>> action,
        TCommand command)
        where TCommand : BaseCommand
    {
        // Arrange
        var exception = new InvalidOperationException("invalid_operation_exception_message");
        var message = "Client made a bad request!";

        // Act
        await GivenException_ShouldReturnsBadRequest(action, command, exception, message);
    }

    protected async Task AggregateNotFoundException_ShouldReturnsBadRequest<TCommand>(
        Func<Task<ActionResult>> action,
        TCommand command)
        where TCommand : BaseCommand
    {
        // Arrange
        var exception = new AggregateNotFoundException("aggregate_not_found_exception_message");
        var message = "Could not retrieve aggregate, client passe incorrect post Id targeting the aggregate!";

        // Act
        await GivenException_ShouldReturnsBadRequest(action, command, exception, message);
    }

    protected void WithHttpPostAttribute(string methodName, string? template = null) =>
                   WithHttpAttribute<HttpPostAttribute>(methodName, template);

    protected void WithHttpPutAttribute(string methodName, string? template = null) =>
                   WithHttpAttribute<HttpPutAttribute>(methodName, template);

    protected void WithHttpGetAttribute(string methodName, string? template = null) =>
                   WithHttpAttribute<HttpGetAttribute>(methodName, template);

    protected void WithHttpDeleteAttribute(string methodName, string? template = null) =>
                   WithHttpAttribute<HttpDeleteAttribute>(methodName, template);

    private void WithHttpAttribute<THttpAttribute>(string methodName, string? template = null)
    where THttpAttribute : HttpMethodAttribute
    {
        // Arrange
        var attributeFunc = (THttpAttribute attribute) => attribute != null && attribute.Template == template;

        // Act
        var decorated = AttributeExtensions.IsDecoratedWithAttribute<TController, THttpAttribute>(attributeFunc, methodName);

        // Assert
        decorated.Should().BeTrue();
    }

    private async Task GivenException_ShouldReturnsBadRequest<TCommand>(
        Func<Task<ActionResult>> action,
        TCommand command,
        Exception exception,
        string message)
        where TCommand : BaseCommand
    {
        // Arrange
        Mock.Get(_dispatcher)
             .Setup(x => x.SendAsync(It.IsAny<BaseCommand>()))
             .ThrowsAsync(exception);

        // Act
        var result = await action() as BadRequestObjectResult;

        // Assert
        result.Should().NotBeNull();
        var response = result!.Value as BaseResponse;
        response!.Message.Should().Be(exception.Message);

        Mock.Get(_dispatcher)
            .Verify(x => x.SendAsync(It.Is<TCommand>(x =>
            command == null ? x != null :
            x == command)

            ), Times.AtLeastOnce);

        Mock.Get(_dispatcher)
            .Verify(x => x.SendAsync(It.Is<TCommand>(x => command == null ? x != null : x == command)), Times.AtLeastOnce);
        _logger.VerifyLoggerWarning(message);
    }
}