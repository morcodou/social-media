using Microsoft.Extensions.Options;
using Post.Command.Infrastructure.Configs;
using Post.Command.Infrastructure.Repositories;

namespace Post.Command.Infrastructure.Test.Repositories
{
    public class EventStoreRepositoryTest
    {
        private readonly EventStoreRepository _sut;
        private readonly IOptions<MongoDbConfig> _configurationOptions;

        public EventStoreRepositoryTest()
        {
            _configurationOptions = Mock.Of<IOptions<MongoDbConfig>>();
            _sut = new EventStoreRepository(_configurationOptions);
        }
    }
}