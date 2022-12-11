namespace Post.Query.Application.Queries;

[ExcludeFromCodeCoverage]
public class FindPostsByAurhorQuery : BaseQuery
{
    public string Author { get; set; } = null!;
}