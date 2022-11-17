using CQRS.Core.Infrastructure;
using CQRS.Core.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Post.Query.Api.Controllers;
using Post.Query.Api.Dtos;
using Post.Query.Api.Queries;
using Post.Query.Api.Test.Extensions;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Test.Controllers
{
    public class PostLookupControllerTest
    {
        private readonly PostLookupController _sut;
        private readonly ILogger<PostLookupController> _logger;
        private readonly IQueryDispatcher<PostEntity> _dispatcher;

        private readonly Fixture _fixture;

        public PostLookupControllerTest()
        {
            _logger = Mock.Of<ILogger<PostLookupController>>();
            _dispatcher = Mock.Of<IQueryDispatcher<PostEntity>>();
            _sut = new(_logger, _dispatcher);

            _fixture = new();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        #region PostLookupController

        [Fact]
        public void PostLookupController_Should_Be_BeAssignableTo_ControllerBase()
        {
            // Assert
            _sut.Should().BeAssignableTo<ControllerBase>();
        }

        [Fact]
        public void PostLookupController_ShouldBe_Decorated_With_ApiControllerAttribute()
        {
            // Arrange
            var attributeFunc = (ApiControllerAttribute attribute) => attribute != null;

            // Act
            var decorated = AttributeExtensions.IsDecoratedWithAttribute<PostLookupController, ApiControllerAttribute>(attributeFunc);

            // Assert
            decorated.Should().BeTrue();
        }

        [Fact]
        public void PostLookupController_ShouldBe_Decorated_With_RouteAttribute()
        {
            // Arrange
            var attributeFunc = (RouteAttribute attribute) => attribute != null && attribute.Template == "api/v1/[controller]";

            // Act
            var decorated = AttributeExtensions.IsDecoratedWithAttribute<PostLookupController, RouteAttribute>(attributeFunc);

            // Assert
            decorated.Should().BeTrue();
        }

        #endregion PostLookupController

        #region GetAllPostsAsync

        [Fact]
        public void GetAllPostsAsync_ShouldBe_Decorated_With_HttpGetAttribute()
        {
            // Arrange
            var getAllPostsAsync = nameof(PostLookupController.GetAllPostsAsync);

            // Act
            WithHttpGetAttribute(getAllPostsAsync);
        }

        [Fact]
        public async Task GetAllPostsAsync_GivenNoPosts_ShouldNoContent()
        {
            // Arrange
            var getResult = () => _sut.GetAllPostsAsync();

            // Act
            await GivenNoPosts<FindAllPostsQuery>(getResult);
        }

        [Fact]
        public async Task GetAllPostsAsync_GivenPosts_ShouldOk()
        {
            // Arrange
            var getResult = () => _sut.GetAllPostsAsync();

            // Act
            await GivenPosts<FindAllPostsQuery>(getResult);
        }

        [Fact]
        public async Task GetAllPostsAsync_GivenException_ShouldInternalServerError()
        {
            // Arrange
            var getResult = () => _sut.GetAllPostsAsync();

            // Act
            await GivenException<FindAllPostsQuery>(getResult);
        }

        #endregion GetAllPostsAsync

        #region GetPostByIdAsync

        [Fact]
        public void GetPostByIdAsync_ShouldBe_Decorated_With_HttpGetAttribute()
        {
            // Arrange
            var template = "byId/{postId}";
            var getPostByIdAsync = nameof(PostLookupController.GetPostByIdAsync);

            // Act
            WithHttpGetAttribute(getPostByIdAsync, template);
        }

        [Fact]
        public async Task GetPostByIdAsync_GivenNoPosts_ShouldNoContent()
        {
            // Arrange
            var Id = Guid.NewGuid();
            var getResult = () => _sut.GetPostByIdAsync(Id);

            // Act
            await GivenNoPosts<FindPostByIdQuery>(getResult);
        }

        [Fact]
        public async Task GetPostByIdAsync_GivenPosts_ShouldOk()
        {
            // Arrange
            var Id = Guid.NewGuid();
            var getResult = () => _sut.GetPostByIdAsync(Id);

            // Act
            await GivenPosts<FindPostByIdQuery>(getResult);
        }

        [Fact]
        public async Task GetPostByIdAsync_GivenException_ShouldInternalServerError()
        {
            // Arrange
            var Id = Guid.NewGuid();
            var getResult = () => _sut.GetPostByIdAsync(Id);

            // Act
            await GivenException<FindPostByIdQuery>(getResult);
        }

        #endregion GetPostByIdAsync

        #region GetPostsByAuhtorAsync

        [Fact]
        public void GetPostsByAuhtorAsync_ShouldBe_Decorated_With_HttpGetAttribute()
        {
            // Arrange
            var template = "byAuthor/{author}";
            var getPostsByAuhtorAsync = nameof(PostLookupController.GetPostsByAuhtorAsync);

            // Act
            WithHttpGetAttribute(getPostsByAuhtorAsync, template);
        }

        [Fact]
        public async Task GetPostsByAuhtorAsync_GivenNoPosts_ShouldNoContent()
        {
            // Arrange
            var author = _fixture.Create<string>();
            var getResult = () => _sut.GetPostsByAuhtorAsync(author);

            // Act
            await GivenNoPosts<FindPostsByAurhorQuery>(getResult);
        }

        [Fact]
        public async Task GetPostsByAuhtorAsync_GivenPosts_ShouldOk()
        {
            // Arrange
            var author = _fixture.Create<string>();
            var getResult = () => _sut.GetPostsByAuhtorAsync(author);

            // Act
            await GivenPosts<FindPostsByAurhorQuery>(getResult);
        }

        [Fact]
        public async Task GetPostsByAuhtorAsync_GivenException_ShouldInternalServerError()
        {
            // Arrange
            var author = _fixture.Create<string>();
            var getResult = () => _sut.GetPostsByAuhtorAsync(author);

            // Act
            await GivenException<FindPostsByAurhorQuery>(getResult);
        }

        #endregion GetPostsByAuhtorAsync

        #region GetPostsWithCommentsAsync

        [Fact]
        public void GetPostsWithCommentsAsync_ShouldBe_Decorated_With_HttpGetAttribute()
        {
            // Arrange
            var template = "withComments";
            var getPostsWithCommentsAsync = nameof(PostLookupController.GetPostsWithCommentsAsync);

            // Act
            WithHttpGetAttribute(getPostsWithCommentsAsync, template);
        }

        [Fact]
        public async Task GetPostsWithCommentsAsync_GivenNoPosts_ShouldNoContent()
        {
            // Arrange
            var getResult = () => _sut.GetPostsWithCommentsAsync();

            // Act
            await GivenNoPosts<FindPostsWithCommentsQuery>(getResult);
        }

        [Fact]
        public async Task GetPostsWithCommentsAsync_GivenPosts_ShouldOk()
        {
            // Arrange
            var getResult = () => _sut.GetPostsWithCommentsAsync();

            // Act
            await GivenPosts<FindPostsWithCommentsQuery>(getResult);
        }

        [Fact]
        public async Task GetPostsWithCommentsAsync_GivenException_ShouldInternalServerError()
        {
            // Arrange
            var getResult = () => _sut.GetPostsWithCommentsAsync();

            // Act
            await GivenException<FindPostsWithCommentsQuery>(getResult);
        }

        #endregion GetPostsWithCommentsAsync

        #region GetPostsWithLikesAsync

        [Fact]
        public void GetPostsWithLikesAsync_ShouldBe_Decorated_With_HttpGetAttribute()
        {
            // Arrange
            var template = "withLikes/{numberOfLikes}";
            var getPostsWithLikesAsync = nameof(PostLookupController.GetPostsWithLikesAsync);

            // Act
            WithHttpGetAttribute(getPostsWithLikesAsync, template);
        }

        [Fact]
        public async Task GetPostsWithLikesAsync_GivenNoPosts_ShouldNoContent()
        {
            // Arrange
            var numberOfLikes = _fixture.Create<int>();
            var getResult = () => _sut.GetPostsWithLikesAsync(numberOfLikes);

            // Act
            await GivenNoPosts<FindPostsWithLikes>(getResult);
        }

        [Fact]
        public async Task GetPostsWithLikesAsync_GivenPosts_ShouldOk()
        {
            // Arrange
            var numberOfLikes = _fixture.Create<int>();
            var getResult = () => _sut.GetPostsWithLikesAsync(numberOfLikes);

            // Act
            await GivenPosts<FindPostsWithLikes>(getResult);
        }

        [Fact]
        public async Task GetPostsWithLikesAsync_GivenException_ShouldInternalServerError()
        {
            // Arrange
            var numberOfLikes = _fixture.Create<int>();
            var getResult = () => _sut.GetPostsWithLikesAsync(numberOfLikes);

            // Act
            await GivenException<FindPostsWithLikes>(getResult);
        }

        #endregion GetPostsWithLikesAsync

        private void WithHttpGetAttribute(string methodName, string? template = null)
        {
            // Arrange
            var attributeFunc = (HttpGetAttribute attribute) => attribute != null && attribute.Template == template;

            // Act
            var decorated = AttributeExtensions.IsDecoratedWithAttribute<PostLookupController, HttpGetAttribute>(attributeFunc, methodName);

            // Assert
            decorated.Should().BeTrue();
        }

        private async Task GivenPosts<TBaseQuery>(Func<Task<ActionResult>> getResultFunc) where TBaseQuery : BaseQuery
        {
            // Arrange
            var posts = _fixture.CreateMany<PostEntity>().ToList();
            Mock.Get(_dispatcher)
                .Setup(f => f.SendAsync(It.IsAny<TBaseQuery>()))
                .ReturnsAsync(posts);

            // Act
            var result = await getResultFunc();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            var lookupResponse = okObjectResult!.Value as PostLookupResponse;
            lookupResponse!.Posts.Should().BeEquivalentTo(posts);
        }

        private async Task GivenNoPosts<TBaseQuery>(Func<Task<ActionResult>> getResultFunc) where TBaseQuery : BaseQuery
        {
            // Arrange
            var posts = Enumerable.Empty<PostEntity>().ToList();
            Mock.Get(_dispatcher)
                .Setup(f => f.SendAsync(It.IsAny<TBaseQuery>()))
                .ReturnsAsync(posts);

            // Act
            var result = await getResultFunc();

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        private async Task GivenException<TBaseQuery>(Func<Task<ActionResult>> getResultFunc) where TBaseQuery : BaseQuery
        {
            // Arrange
            var posts = _fixture.CreateMany<PostEntity>().ToList();
            Mock.Get(_dispatcher)
                .Setup(f => f.SendAsync(It.IsAny<TBaseQuery>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await getResultFunc();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result;
            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}