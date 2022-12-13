namespace Post.Query.Application.Queries;

[ExcludeFromCodeCoverage]
public class FindPostByIdQuery : BaseQuery
{
    public Guid Id { get; set; }
}