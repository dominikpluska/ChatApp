using MongoDB.Driver;
using UserSettingsApi.Data;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.ChatsRepository
{
    public class ChatsRepository : IChatsRepository
    {
        private readonly MongoDBService _mongoDBService;

        public ChatsRepository(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<Chat> GetChatList(string userId)
        {

           var filter = Builders<Chat>.Filter.Eq(x => x.UserId, userId);
           var chatsList = await _mongoDBService.ChatsCollection.Find(filter).FirstOrDefaultAsync();
           return chatsList;

        }
    }
}
