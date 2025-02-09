using MessagesApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MessagesApi.Data
{
    public class MongoDBService
    {
        private readonly IConfiguration _configuration;

        public IMongoCollection<Chat> ChatCollection { get; init; }
        public IMongoCollection<BsonDocument> ChatCollectionBson { get; init; }
        public MongoDBService(IConfiguration configuration)
        {
            _configuration = configuration;
            MongoClient client = new MongoClient(_configuration.GetValue<string>("MongoDbSettings:ConnectionString")!);

            IMongoDatabase database = client.GetDatabase(_configuration.GetValue<string>("MongoDbSettings:DatabaseName")!);

            ChatCollection = database.GetCollection<Chat>(_configuration.GetValue<string>("MongoDbSettings:CollectionName")!);

            ChatCollectionBson = database.GetCollection<BsonDocument>(_configuration.GetValue<string>("MongoDbSettings:CollectionName")!);

        }
    }
}
