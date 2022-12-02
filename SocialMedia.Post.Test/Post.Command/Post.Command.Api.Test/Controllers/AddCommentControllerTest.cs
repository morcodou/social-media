using Microsoft.AspNetCore.Mvc;
using Post.Command.Api.Commands;
using Post.Command.Api.Controllers;
using Post.Command.Api.Dtos;

namespace Post.Command.Api.Test.Controllers
{
    public class AddCommentControllerTest : ControllerFixtureBase<AddCommentController>
    {
        private readonly AddCommentController _sut;
        private const string _METHODENAME = nameof(AddCommentController.AddCommentAsync);
        private const string _ATTRIBUTETEMPLATE = "{id}";

        public AddCommentControllerTest()
        {
            _sut = new(_logger, _dispatcher);
        }

        [Fact]
        public void AddCommentController_ShouldBe_Decorated_With_ApiControllerAttribute() =>
                    Controller_ShouldBe_Decorated_With_ApiControllerAttribute();

        [Fact]
        public void AddCommentController_ShouldBe_Decorated_With_RouteAttribute() =>
                    Controller_ShouldBe_Decorated_With_RouteAttribute();

        [Fact]
        public void AddCommentAsync_ShouldBe_Decorated_With_HttpGetAttribute() =>
                    WithHttpPutAttribute(_METHODENAME, _ATTRIBUTETEMPLATE);

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
            var action = () => _sut.AddCommentAsync(id, command);
            var message = "An error occurred while processing request to add comment!";

            // Act
            await UnhandleException_ShouldReturnsInternalServerError(action, command, message);
        }

        [Fact]
        public async Task AddCommentAsync_GivenInvalidOperationException_ShouldReturnsBadRequest()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            var command = _fixture.Create<AddCommentCommand>();
            var action = () => _sut.AddCommentAsync(id, command);

            // Act
            await InvalidOperationException_ShouldReturnsBadRequest(action, command);
        }

        [Fact]
        public async Task AddCommentAsync_GivenAggregateNotFoundException_ShouldReturnsBadRequest()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            var command = _fixture.Create<AddCommentCommand>();
            var action = () => _sut.AddCommentAsync(id, command);
            var message = "Could not retrieve aggregate, client passe incorrect post Id targeting the aggregate!";

            // Act
            await AggregateNotFoundException_ShouldReturnsBadRequest(action, command, message);
        }
    }
}