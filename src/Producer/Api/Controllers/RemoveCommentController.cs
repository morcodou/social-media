namespace Post.Command.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RemoveCommentController : ControllerBase
{
    private readonly ILogger<RemoveCommentController> _logger;
    private readonly ICommandDispatcher _dispatcher;
    public RemoveCommentController(
        ILogger<RemoveCommentController> logger,
        ICommandDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveCommentAsync(Guid id, RemoveCommentCommand command)
    {
        try
        {
            command.Id = id;
            await _dispatcher.SendAsync(command);

            return Ok(new BaseResponse()
            {
                Message = "Remove comment request completed successfully!"
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
            var errorMessage = "An error occurred while processing request to remove comment!";
            _logger.LogError(exception, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new PostResponse()
            {
                Id = id,
                Message = errorMessage
            });
        }
    }
}