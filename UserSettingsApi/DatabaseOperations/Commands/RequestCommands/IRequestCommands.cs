using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Commands.FriendRequestCommands
{
    public interface IRequestCommands
    {
        public Task<IResult> InsertRequests(Request request);
        public Task<IResult> DeleteRequest(ObjectId requestId);
        public Task<IResult> AcceptRequest(ObjectId requestId);
    }
}
