namespace Post.Common.Dtos
{
    public class BaseResponse
    {
        public string Message { get; set; } = null!;
        public BaseResponse(string message) => Message = message;
        public BaseResponse() { }
    }
}