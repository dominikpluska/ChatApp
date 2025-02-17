using UserSettingsApi.Dto;

namespace UserSettingsApi.Managers.BlackListsManager
{
    public interface IBlackListsManager
    {
        public Task<IResult> CreateBlackListTable(string userId);
        public Task<IResult> GetBlackList();

        public Task<IResult> AddUserToBlackList(string blockedUserId);
        public Task<IResult> RemoveFromBlackList(string blockedUserId);
    }
}
