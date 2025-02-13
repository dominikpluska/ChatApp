using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.BlackListRepository
{
    public interface IBlackListRepository
    {
        public Task<BlackList> GetBlackList(string userId);
    }
}
