using Post.Common.Dtos;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Dtos
{
    public class PostLookupResponse : BaseResponse
    {
        public List<PostEntity> Posts { get; set; } = new();
    }
}