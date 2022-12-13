namespace Post.Query.Application.Queries;

[ExcludeFromCodeCoverage]
public class FindPostsWithLikesQuery : BaseQuery
{
    public int NumberOfLikes { get; set; }
}
