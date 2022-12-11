using System.Collections.Generic;

namespace Post.Query.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PostLookupController : ControllerBase
{
    private readonly ILogger<PostLookupController> _logger;
    private readonly IQueryDispatcher<PostEntity> _dispatcher;
    public PostLookupController(
        ILogger<PostLookupController> logger,
        IQueryDispatcher<PostEntity> dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    [HttpGet()]
    public async Task<ActionResult> GetAllPostsAsync()
    {
        try
        {
            var query = new FindAllPostsQuery();
            var posts = await _dispatcher.SendAsync(query);
            if (posts.IsNullOrEmpty())
                return NoContent();
            var count = posts.Count();

            return Ok(new PostLookupResponse()
            {
                Posts = posts,
                Message = $"successfully returned {count} post {(count > 1 ? "s" : string.Empty)}!"
            });
        }
        catch (Exception exception)
        {
            var errorMessage = "An error occurred while processing request to retrieving all posts!";
            _logger.LogError(exception, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = errorMessage
            });
        }
    }

    [HttpGet("byId/{postId}")]
    public async Task<ActionResult> GetPostByIdAsync(Guid postId)
    {
        try
        {
            var query = new FindPostByIdQuery() { Id = postId };
            var posts = await _dispatcher.SendAsync(query);
            if (posts.IsNullOrEmpty())
                return NoContent();

            return Ok(new PostLookupResponse()
            {
                Posts = posts,
                Message = $"successfully returned post!"
            });
        }
        catch (Exception exception)
        {
            var errorMessage = "An error occurred while processing request to retrieving a post by Id!";
            _logger.LogError(exception, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = errorMessage
            });
        }
    }


    [HttpGet("byAuthor/{author}")]
    public async Task<ActionResult> GetPostsByAuhtorAsync(string author)
    {
        try
        {
            var query = new FindPostsByAurhorQuery() { Author = author };
            var posts = await _dispatcher.SendAsync(query);
            if (posts.IsNullOrEmpty())
                return NoContent();
            var count = posts.Count();

            return Ok(new PostLookupResponse()
            {
                Posts = posts,
                Message = $"successfully returned {count} post {(count > 1 ? "s" : string.Empty)} by {author}!"
            });
        }
        catch (Exception exception)
        {
            var errorMessage = "An error occurred while processing request to retrieving posts by author!";
            _logger.LogError(exception, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = errorMessage
            });
        }
    }

    [HttpGet("withComments")]
    public async Task<ActionResult> GetPostsWithCommentsAsync()
    {
        try
        {
            var query = new FindPostsWithCommentsQuery() ;
            var posts = await _dispatcher.SendAsync(query);
            if (posts.IsNullOrEmpty())
                return NoContent();
            var count = posts.Count();

            return Ok(new PostLookupResponse()
            {
                Posts = posts,
                Message = $"successfully returned {count} post {(count > 1 ? "s" : string.Empty)} with comments!"
            });
        }
        catch (Exception exception)
        {
            var errorMessage = "An error occurred while processing request to retrieving posts with comments!";
            _logger.LogError(exception, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = errorMessage
            });
        }
    }

    [HttpGet("withLikes/{numberOfLikes}")]
    public async Task<ActionResult> GetPostsWithLikesAsync(int numberOfLikes)
    {
        try
        {
            var query = new FindPostsWithLikesQuery() { NumberOfLikes = numberOfLikes} ;
            var posts = await _dispatcher.SendAsync(query);
            if (posts.IsNullOrEmpty())
                return NoContent();
            var count = posts.Count();

            return Ok(new PostLookupResponse()
            {
                Posts = posts,
                Message = $"successfully returned {count} post {(count > 1 ? "s" : string.Empty)} with likes!"
            });
        }
        catch (Exception exception)
        {
            var errorMessage = "An error occurred while processing request to retrieving posts with likes!";
            _logger.LogError(exception, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = errorMessage
            });
        }
    }
}