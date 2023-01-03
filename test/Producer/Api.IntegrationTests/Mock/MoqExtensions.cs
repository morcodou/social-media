using Post.Command.Application.Infrastructure;
using Post.Command.Infrastructure.Models;
using Post.Command.Infrastructure.MongoCollection;

namespace Moq;

internal static class MoqExtensions
{
    public static IEventProducer GetEventProducer()
    {
        return Mock.Of<IEventProducer>();
    }

    public static IMongoEventCollection<EventModel> GetMongoEventModelCollection()
    {
        return Mock.Of<IMongoEventCollection<EventModel>>();
    }
}