namespace Post.Query.Application.Queries;

public interface IQueryHandler
{
    Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query);
    Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query);
    Task<List<PostEntity>> HandleAsync(FindPostsByAurhorQuery query);
    Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query);
    Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query);
}