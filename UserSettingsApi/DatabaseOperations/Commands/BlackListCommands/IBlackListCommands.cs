using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Commands.BlackListCommands
{
    public interface IBlackListCommands
    {
        public Task<IResult> CreateBlackList(BlackList blackList);
    }
}
