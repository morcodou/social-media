namespace Post.Command.Infrastructure.MongoCollection;

public interface IMongoEventCollection<TDocument>
{
    Task InsertOneAsync(TDocument document);

    Task<List<TDocument>> Find(Func<TDocument, bool> filter);
}
