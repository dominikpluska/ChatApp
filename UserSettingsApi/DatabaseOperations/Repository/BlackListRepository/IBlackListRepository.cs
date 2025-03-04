using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.BlackListRepository
{
    public interface IBlackListRepository
    {
        public Task<BlackList> GetBlackList(string userId, CancellationToken cancellationToken);

        public Task<ObjectId> GetBlackListId(string userId, CancellationToken cancellationToken);
        public Task<string> GetBlockedUser(ObjectId blackListId, string blockedId, CancellationToken cancellationToken);
    }
}
