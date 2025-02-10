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
            try
            {
                var filter = Builders<Chat>.Filter.Eq(x => x.UserId, userId);
                var chatsList = await _mongoDBService.ChatsCollection.Find(filter).FirstOrDefaultAsync();
                return chatsList;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
