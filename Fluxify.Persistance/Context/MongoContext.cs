using Fluxify.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Persistance.Context
{
    public sealed class MongoContext
    {
        public IMongoDatabase Database { get; }

        public MongoContext(IMongoClient client)
        {
            Database = client.GetDatabase(AppConfiguration.MongoDbOptions.DataBase);
        }
    }
}
