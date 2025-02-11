using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using UserSettingsApi.Models;

namespace UserSettingsApi.Data
{
    public class MongoDBService
    {
        private readonly IConfiguration _configuration;

        public IMongoCollection<FriendsList> FriendsListCollection { get; init; }
        public IMongoCollection<BsonDocument> FriendsListCollectionBson { get; init; }
        public IMongoCollection<Chat> ChatsCollection { get; init; }
        public IMongoCollection<BsonDocument> ChatsCollectionBson { get; init; }
        public IMongoCollection<BlackList> BlackListsCollection { get; init; }
        public IMongoCollection<BsonDocument> BlackListsCollectionBson { get; init; }

        public MongoDBService(IConfiguration configuration)
        {
            _configuration = configuration;
            MongoClient client = new MongoClient(_configuration.GetValue<string>("MongoDbSettings:ConnectionString")!);
            IMongoDatabase database = client.GetDatabase(_configuration.GetValue<string>("MongoDbSettings:DatabaseName")!);

            string[] mongoCollections = _configuration.GetSection("MongoDbSettings:CollectionNames").Get<string[]>()!;

            FriendsListCollection = database.GetCollection<FriendsList>(mongoCollections[0]);
            FriendsListCollectionBson = database.GetCollection<BsonDocument>(mongoCollections[0]);
            ChatsCollection = database.GetCollection<Chat>(mongoCollections[1]);
            ChatsCollectionBson = database.GetCollection<BsonDocument>(mongoCollections[1]);
            BlackListsCollection = database.GetCollection<BlackList>(mongoCollections[2]);
            BlackListsCollectionBson = database.GetCollection<BsonDocument>(mongoCollections[2]);
        }
    }
}
