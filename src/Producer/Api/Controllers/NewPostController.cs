namespace Post.Command.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class NewPostController : CommandControllerBase<NewPostController>
{
    public NewPostController(ILogger<NewPostController> logger, ICommandDispatcher dispatcher)
        : base(logger, dispatcher)
    {
    }

    [HttpPost]
    public async Task<ActionResult> NewPostAsync(NewPostCommand command)
    {
        var id = Guid.NewGuid();
        try
        {
            command.Id = id;
            await _dispatcher.SendAsync(command);

            return StatusCode(StatusCodes.Status201Created, new PostResponse()
            {
                Id = id,
                Message = "New post creation request completed successfully!"
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
        catch (Exception exception)
        {
            var errorMessage = "An error occurred while processing request to create a new post!";
            _logger.LogError(exception, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new PostResponse()
            {
                Id = id,
                Message = errorMessage
            });
        }
    }
}