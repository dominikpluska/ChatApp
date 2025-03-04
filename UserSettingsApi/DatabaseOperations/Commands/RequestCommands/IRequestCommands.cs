using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Commands.FriendRequestCommands
{
    public interface IRequestCommands
    {
        public Task<IResult> InsertRequests(Request request, CancellationToken cancellationToken);
        public Task<IResult> DeleteRequest(ObjectId requestId, CancellationToken cancellationToken);
        public Task<IResult> AcceptRequest(ObjectId requestId, CancellationToken cancellationToken);
    }
}
