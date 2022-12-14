namespace Post.Query.Application.Handlers;

public class QueryHandler : IQueryHandler
{
    private readonly IPostRepository _postRepository;
    public QueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    public async Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query) =>
        await _postRepository.ListAllAsync();

    public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
    {
        var postEntity = await _postRepository.GetByIdAsync(query.Id);
        var postEntities = new List<PostEntity>() { postEntity };
        return postEntities.OfType<PostEntity>().ToList();
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostsByAurhorQuery query) =>
        await _postRepository.ListByAuthorAsync(query.Author);

    public async Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query) =>
        await _postRepository.ListWithCommentsAsync();

    public async Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query) =>
        await _postRepository.ListWithLikesAsync(query.NumberOfLikes);
}