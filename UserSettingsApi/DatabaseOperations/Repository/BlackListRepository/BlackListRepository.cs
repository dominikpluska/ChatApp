using MongoDB.Bson;
using MongoDB.Driver;
using UserSettingsApi.Data;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.BlackListRepository
{
    public class BlackListRepository : IBlackListRepository
    {
        private readonly MongoDBService _mongoDBService;

        public BlackListRepository(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<BlackList> GetBlackList(string userId)
        {
            try
            {
                var filter = Builders<BlackList>.Filter.Eq(x => x.UserAccountId, userId);
                var chatsList = await _mongoDBService.BlackListsCollection.Find(filter).FirstOrDefaultAsync();
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
