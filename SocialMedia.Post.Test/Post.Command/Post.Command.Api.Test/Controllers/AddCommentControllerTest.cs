using CQRS.Core.Commands;
using CQRS.Core.Exceptions;
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
    public class AddCommentControllerTest : ControllerFixtureBase
    {
        private readonly AddCommentController _sut;
        private readonly ILogger<AddCommentController> _logger;
        private readonly ICommandDispatcher _dispatcher;
        private readonly Fixture _fixture;
        public AddCommentControllerTest()
        {
            _logger = Mock.Of<ILogger<AddCommentController>>();
            _dispatcher = Mock.Of<ICommandDispatcher>();
            _sut = new(_logger, _dispatcher);

            _fixture = new();
        }

        [Fact]
        public void AddCommentController_ShouldBe_Decorated_With_ApiControllerAttribute()
        {
            // Arrange
            var attributeFunc = (ApiControllerAttribute attribute) => attribute != null;

            // Act
            var decorated = AttributeExtensions.IsDecoratedWithAttribute<AddCommentController, ApiControllerAttribute>(attributeFunc);

            // Assert
            decorated.Should().BeTrue();
        }

        [Fact]
        public void AddCommentController_ShouldBe_Decorated_With_RouteAttribute()
        {
            // Arrange
            var attributeFunc = (RouteAttribute attribute) => attribute != null && attribute.Template == "api/v1/[controller]";

            // Act
            var decorated = AttributeExtensions.IsDecoratedWithAttribute<AddCommentController, RouteAttribute>(attributeFunc);

            // Assert
            decorated.Should().BeTrue();
        }

        [Fact]
        public void AddCommentAsync_ShouldBe_Decorated_With_HttpGetAttribute()
        {
            // Arrange
            var template = "{id}";
            var addCommentAsync = nameof(AddCommentController.AddCommentAsync);

            // Act
            WithHttpPutAttribute<AddCommentController>(addCommentAsync, template);
        }

        [Fact]
        public async Task AddCommentAsync_GivenIdAndAddCommentCommand_ShouldReturnsOkResponse()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            var command = _fixture.Create<AddCommentCommand>();
            var commentPostResponse = new CommentPostResponse()
            {
                Id = id,
                Message = "Add comment request completed successfully!"
            };

            // Act
            var result = await _sut.AddCommentAsync(id, command) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.Value.Should().BeEquivalentTo(commentPostResponse);
            command.Id.Should().Be(id);
            Mock.Get(_dispatcher)
                .Verify(x => x.SendAsync(command), Times.Once);
        }

        [Fact]
        public async Task AddCommentAsync_GivenUnhandleException_ShouldReturnsInternalServerError()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            var command = _fixture.Create<AddCommentCommand>();
            var exception = new Exception("unhandle_exception");
            var message = "An error occurred while processing request to add comment!";
            Mock.Get(_dispatcher)
                 .Setup(x => x.SendAsync(It.IsAny<BaseCommand>()))
                 .ThrowsAsync(exception);

            // Act
            var result = await _sut.AddCommentAsync(id, command) as ObjectResult;

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
        public async Task AddCommentAsync_GivenInvalidOperationException_ShouldReturnsBadRequest()
        {
            // Arrange
            var exception = new InvalidOperationException("invalid_operation_exception_message");
            var message = "Client made a bad request!";

            // Act
            await AddCommentAsync_ShouldReturnsBadRequest(exception, message);
        }

        [Fact]
        public async Task AddCommentAsync_GivenAggregateNotFoundException_ShouldReturnsBadRequest()
        {
            // Arrange
            var exception = new AggregateNotFoundException("aggregate_not_found_exception_message");
            var message = "Could not retrieve aggregate, client passe incorrect post Id targeting the aggregate!";

            // Act
            await AddCommentAsync_ShouldReturnsBadRequest(exception, message);
        }

        private async Task AddCommentAsync_ShouldReturnsBadRequest(Exception exception, string message)
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            var command = _fixture.Create<AddCommentCommand>();
            Mock.Get(_dispatcher)
                 .Setup(x => x.SendAsync(It.IsAny<BaseCommand>()))
                 .ThrowsAsync(exception);

            // Act
            var result = await _sut.AddCommentAsync(id, command) as BadRequestObjectResult;

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