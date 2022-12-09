
using Microsoft.Extensions.Options;
using Post.Command.Infrastructure.Configuration;

namespace Post.Command.Infrastructure.Repositories;

public class EventStoreRepositoryTest
{
    private readonly EventStoreRepository _sut;
    private readonly IOptions<MongoDbConfiguration> _configurationOptions;

    public EventStoreRepositoryTest()
    {
        _configurationOptions = Mock.Of<IOptions<MongoDbConfiguration>>();
        _sut = new EventStoreRepository(_configurationOptions);
    }
}