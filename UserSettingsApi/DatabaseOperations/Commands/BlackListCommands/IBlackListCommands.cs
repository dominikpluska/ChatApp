using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Commands.BlackListCommands
{
    public interface IBlackListCommands
    {
        public Task<IResult> CreateBlackList(BlackList blackList);
        public Task<IResult> AddToBlackList(ObjectId blackListId, string userId);
        public Task<IResult> RemoveFromBlackList(ObjectId blackListId, string userId);
    }
}
