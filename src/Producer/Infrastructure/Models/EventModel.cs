using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Post.Command.Infrastructure.Models;

[ExcludeFromCodeCoverage]
public class EventModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }= null!;
    public DateTime TimeStamp { get; set; }
    public Guid AggregateIdentifier { get; set; }
    public string AggregateType { get; set; } = null!;
    public int Version { get; set; }
    public string EventType { get; set; }= null!;
    public BaseEvent EventData { get; set; }= null!;
}