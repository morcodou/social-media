namespace Post.Query.Api.Dtos;

public class PostLookupResponse : BaseResponse
{
    public List<PostEntity> Posts { get; set; } = new();
}