namespace Post.Command.Api.Dtos;

public class BaseResponse
{
    public string Message { get; set; } = null!;
    public BaseResponse(string message) => Message = message;
    public BaseResponse() { }
}