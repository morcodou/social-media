using Microsoft.AspNetCore.Mvc;
using Post.Command.Api.Commands;
using Post.Command.Api.Controllers;
using Post.Common.Dtos;

namespace Post.Command.Api.Test.Controllers
{
    public class DeletePostControllerTest : ControllerFixtureBase<DeletePostController>
    {
        private readonly DeletePostController _sut;
        private const string _METHODENAME = nameof(DeletePostController.DeletePostAsync);
        private const string _ATTRIBUTETEMPLATE = "{id}";

        public DeletePostControllerTest() => _sut = new(_logger, _dispatcher);

        [Fact]
        public void DeletePostController_ShouldBe_Decorated_With_ApiControllerAttribute() =>
                    Controller_ShouldBe_Decorated_With_ApiControllerAttribute();

        [Fact]
        public void DeletePostController_ShouldBe_Decorated_With_RouteAttribute() =>
                    Controller_ShouldBe_Decorated_With_RouteAttribute();

        [Fact]
        public void DeletePostAsync_ShouldBe_Decorated_With_HttpGetAttribute() =>
                    WithHttpDeleteAttribute(_METHODENAME, _ATTRIBUTETEMPLATE);

        [Fact]
        public async Task DeletePostAsync_GivenIdAndDeletePostCommand_ShouldReturnsOkResponse()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            var command = _fixture.Create<DeletePostCommand>();
            var response = new BaseResponse("Remove post request completed successfully!");

            // Act
            var result = await _sut.DeletePostAsync(id, command) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.Value.Should().BeEquivalentTo(response);
            command.Id.Should().Be(id);
            Mock.Get(_dispatcher)
                .Verify(x => x.SendAsync(command), Times.Once);
        }

        [Fact]
        public async Task DeletePostAsync_GivenUnhandleException_ShouldReturnsInternalServerError()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            var command = _fixture.Create<DeletePostCommand>();
            var action = () => _sut.DeletePostAsync(id, command);
            var message = "An error occurred while processing request to remove post!";

            // Act
            await UnhandleException_ShouldReturnsInternalServerError(action, command, message);
        }

        [Fact]
        public async Task DeletePostAsync_GivenInvalidOperationException_ShouldReturnsBadRequest()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            var command = _fixture.Create<DeletePostCommand>();
            var action = () => _sut.DeletePostAsync(id, command);

            // Act
            await InvalidOperationException_ShouldReturnsBadRequest(action, command);
        }

        [Fact]
        public async Task DeletePostAsync_GivenAggregateNotFoundException_ShouldReturnsBadRequest()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            var command = _fixture.Create<DeletePostCommand>();
            var action = () => _sut.DeletePostAsync(id, command);

            // Act
            await AggregateNotFoundException_ShouldReturnsBadRequest(action, command);
        }
    }
}