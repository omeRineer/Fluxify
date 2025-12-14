using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Persistance
{
    public class FluxifyDbContext
    {
        public IMongoDatabase Database { get; }
        public MongoClient Client { get; }


        public FluxifyDbContext(string connectionString, string databaseName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("MongoDB connection string is required", nameof(connectionString));


            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentException("MongoDB database name is required", nameof(databaseName));


            var settings = MongoClientSettings.FromConnectionString(connectionString);
            // configure recommended settings here, e.g., retry writes
            settings.RetryWrites = true;


            Client = new MongoClient(settings);
            Database = Client.GetDatabase(databaseName);
        }


        // Optional: helper to start a session for transactions
        public IClientSessionHandle StartSession() => Client.StartSession();
    }
}
