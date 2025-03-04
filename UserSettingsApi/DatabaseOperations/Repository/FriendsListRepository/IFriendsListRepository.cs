using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.FriendsListRepository
{
    public interface IFriendsListRepository
    {
        public Task<FriendsList> GetFriendsList(string userId, CancellationToken cancellationToken);
        public Task<ObjectId> GetFriendsListId(string userId, CancellationToken cancellationToken);
    }
}
