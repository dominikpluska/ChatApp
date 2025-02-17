using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.BlackListRepository
{
    public interface IBlackListRepository
    {
        public Task<BlackList> GetBlackList(string userId);

        public Task<ObjectId> GetBlackListId(string userId);
        public Task<string> GetBlockedUser(ObjectId blackListId, string blockedId);
    }
}
