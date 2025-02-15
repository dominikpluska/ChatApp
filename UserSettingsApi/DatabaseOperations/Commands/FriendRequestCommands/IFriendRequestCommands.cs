using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Commands.FriendRequestCommands
{
    public interface IFriendRequestCommands
    {
        public Task<IResult> InsertFriendRequests(Request friendRequest);
        public Task<IResult> DeleteFriendRequest(ObjectId friendRequestId);
        public Task<IResult> AcceptFriendRequest(ObjectId friendRequestId);
    }
}
