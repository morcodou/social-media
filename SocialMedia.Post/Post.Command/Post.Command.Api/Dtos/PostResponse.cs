using Post.Common.Dtos;

namespace Post.Command.Api.Dtos
{
    public class PostResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}