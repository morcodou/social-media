namespace Post.Command.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LikePostController : ControllerBase
{
    private readonly ILogger<LikePostController> _logger;
    private readonly ICommandDispatcher _dispatcher;
    public LikePostController(
        ILogger<LikePostController> logger,
        ICommandDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> LikePostAsync(Guid id)
    {
        try
        {
            var command = new LikePostCommand() { Id = id };
            await _dispatcher.SendAsync(command);

            return Ok(new PostResponse()
            {
                Id = id,
                Message = "Like post request completed successfully!"
            });
        }
        catch (InvalidOperationException exception)
        {
            _logger.LogWarning(exception, "Client made a bad request!");
            return BadRequest(new BaseResponse()
            {
                Message = exception.Message
            });
        }
        catch (AggregateNotFoundException exception)
        {
            _logger.LogWarning(exception, "Could not retrieve aggregate, client passe incorrect post Id targeting the aggregate!");
            return BadRequest(new BaseResponse()
            {
                Message = exception.Message
            });
        }
        catch (Exception exception)
        {
            var errorMessage = "An error occurred while processing request to like a post!";
            _logger.LogError(exception, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new PostResponse()
            {
                Id = id,
                Message = errorMessage
            });
        }
    }
}