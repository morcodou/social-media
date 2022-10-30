using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Post.Command.Infrastructure.Configs
{
    public class MongoDbConfig
    {
        public string ConnectionString { get; set; } = null!;
        public string Database { get; set; } = null!;
        public string Collection { get; set; } = null!;
    }
}