using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Commands.BlackListCommands
{
    public interface IBlackListCommands
    {
        public Task<IResult> CreateBlackList(BlackList blackList, CancellationToken cancellationToken);
        public Task<IResult> AddToBlackList(ObjectId blackListId, string userId, CancellationToken cancellationToken);
        public Task<IResult> RemoveFromBlackList(ObjectId blackListId, string userId, CancellationToken cancellationToken);
    }
}
