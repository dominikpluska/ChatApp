using UserSettingsApi.Dto;

namespace UserSettingsApi.Managers.BlackListsManager
{
    public interface IBlackListsManager
    {
        public Task<IResult> CreateBlackListTable(string userId, CancellationToken cancellationToken);
        public Task<IResult> GetBlackList(CancellationToken cancellationToken);
        public Task<IResult> GetUserFromBlackList(string userId, string chatterId, CancellationToken cancellationToken);
        public Task<IResult> AddUserToBlackList(string blockedUserId, CancellationToken cancellationToken);
        public Task<IResult> RemoveFromBlackList(string blockedUserId, CancellationToken cancellationToken);
    }
}
