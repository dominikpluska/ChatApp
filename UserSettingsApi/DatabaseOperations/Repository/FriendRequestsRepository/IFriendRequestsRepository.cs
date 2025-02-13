using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.FriendRequestsRepository
{
    public interface IFriendRequestsRepository
    {
        public Task<FriendRequest> GetFriendRequest(ObjectId friendRequestId);
    }
}
