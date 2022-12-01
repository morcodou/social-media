using CQRS.Core.Commands;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Post.Command.Api.Commands;
using Post.Command.Api.Controllers;
using Post.Command.Api.Dtos;
using Post.Common.Dtos;

namespace Post.Command.Api.Test.Controllers
{
    public class NewPostControllerTest : ControllerFixtureBase
    {
        private readonly NewPostController _sut;
        private readonly ILogger<NewPostController> _logger;
        private readonly ICommandDispatcher _dispatcher;
        private readonly Fixture _fixture;
        public NewPostControllerTest()
        {
            _logger = Mock.Of<ILogger<NewPostController>>();
            _dispatcher = Mock.Of<ICommandDispatcher>();
            _sut = new(_logger, _dispatcher);

            _fixture = new();
        }

        [Fact]
        public void NewPostController_ShouldBe_Decorated_With_ApiControllerAttribute()
        {
            // Arrange
            var attributeFunc = (ApiControllerAttribute attribute) => attribute != null;

            // Act
            var decorated = AttributeExtensions.IsDecoratedWithAttribute<NewPostController, ApiControllerAttribute>(attributeFunc);

            // Assert
            decorated.Should().BeTrue();
        }

        [Fact]
        public void NewPostController_ShouldBe_Decorated_With_RouteAttribute()
        {
            // Arrange
            var attributeFunc = (RouteAttribute attribute) => attribute != null && attribute.Template == "api/v1/[controller]";

            // Act
            var decorated = AttributeExtensions.IsDecoratedWithAttribute<NewPostController, RouteAttribute>(attributeFunc);

            // Assert
            decorated.Should().BeTrue();
        }

        [Fact]
        public void NewPostAsync_ShouldBe_Decorated_With_HttpAttribute()
        {
            // Arrange
            var newPostAsync = nameof(NewPostController.NewPostAsync);

            // Act
            WithHttpPostAttribute<NewPostController>(newPostAsync);
        }

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
            var exception = new Exception("unhandle_exception");
            var message = "An error occurred while processing request to create a new post!";
            Mock.Get(_dispatcher)
                 .Setup(x => x.SendAsync(It.IsAny<BaseCommand>()))
                 .ThrowsAsync(exception);

            // Act
            var result = await _sut.NewPostAsync(command) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            var response = result!.Value as BaseResponse;
            response!.Message.Should().Be(message);
            Mock.Get(_dispatcher)
                .Verify(x => x.SendAsync(command), Times.Once);
            _logger.VerifyLoggerError(message);
        }

        [Fact]
        public async Task NewPostAsync_GivenInvalidOperationException_ShouldReturnsBadRequest()
        {
            // Arrange
            var exception = new InvalidOperationException("invalid_operation_exception_message");
            var message = "Client made a bad request!";

            // Act
            await NewPostAsync_ShouldReturnsBadRequest(exception, message);
        }

        private async Task NewPostAsync_ShouldReturnsBadRequest(Exception exception, string message)
        {
            // Arrange
            var command = _fixture.Create<NewPostCommand>();
            Mock.Get(_dispatcher)
                 .Setup(x => x.SendAsync(It.IsAny<BaseCommand>()))
                 .ThrowsAsync(exception);

            // Act
            var result = await _sut.NewPostAsync(command) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            var response = result!.Value as BaseResponse;
            response!.Message.Should().Be(exception.Message);
            Mock.Get(_dispatcher)
                .Verify(x => x.SendAsync(command), Times.Once);
            _logger.VerifyLoggerWarning(message);
        }
    }
}