using System.Linq.Expressions;

namespace Post.Command.Infrastructure.MongoCollection;

public interface IMongoEventCollection<TDocument>
{
    Task InsertOneAsync(TDocument document);

    Task<List<TDocument>> Find(Expression<Func<TDocument, bool>> filter);
}
