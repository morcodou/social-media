namespace Post.Command.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AddCommentController : CommandControllerBase<AddCommentController>
{
    public AddCommentController(ILogger<AddCommentController> logger, ICommandDispatcher dispatcher)
            : base(logger, dispatcher)
    {
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> AddCommentAsync(Guid id, AddCommentCommand command)
    {
        try
        {
            command.Id = id;
            await _dispatcher.SendAsync(command);

            return Ok(new CommentPostResponse()
            {
                Id = id,
                Message = "Add comment request completed successfully!"
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
            var errorMessage = "An error occurred while processing request to add comment!";
            _logger.LogError(exception, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = errorMessage
            });
        }
    }
}