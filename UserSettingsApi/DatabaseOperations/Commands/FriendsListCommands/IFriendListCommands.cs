using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Commands.FriendsLisiCommands
{
    public interface IFriendListCommands
    {
        public Task<IResult> CreateFriendList(FriendsList friendsList, CancellationToken cancellationToken);
        public Task<IResult> AddNewFriend(ObjectId friendsListId, string friendId, CancellationToken cancellationToken);
        public Task<IResult> RemoveFriend(ObjectId friendsListId, string friendId, CancellationToken cancellationToken);
    }
}
