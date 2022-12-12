namespace Post.Query.Api.Dtos;

[ExcludeFromCodeCoverage]
public class PostLookupResponse : BaseResponse
{
    public List<PostEntity> Posts { get; set; } = new();
}