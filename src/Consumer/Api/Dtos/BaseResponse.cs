using System.Diagnostics.CodeAnalysis;
namespace Post.Query.Api.Dtos;

[ExcludeFromCodeCoverage]
public class BaseResponse
{
    public string Message { get; set; } = null!;
    public BaseResponse(string message) => Message = message;
    public BaseResponse() { }
}