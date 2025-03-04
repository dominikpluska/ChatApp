using UserSettingsApi.Dto;

namespace UserSettingsApi.Managers.ChatsManager
{
    public interface IChatsManager
    {
        public Task<IResult> GetAllChats(CancellationToken cancellationToken);
        public Task<IResult> CreateChatsTable(string userId, CancellationToken cancellationToken);
    }
}
