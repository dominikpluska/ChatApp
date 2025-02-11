using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.FriendsListRepository
{
    public interface IFriendsListRepository
    {
        public Task<FriendsList> GetFriendsList(string userId);
        public Task<ObjectId> GetFriendsListId(string userId);
    }
}
