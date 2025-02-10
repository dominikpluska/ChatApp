using UserSettingsApi.Dto;

namespace UserSettingsApi.Managers.ChatsManager
{
    public interface IChatsManager
    {
        public Task<IResult> GetAllChats();
        public Task<IResult> CreateChatsTable(UserSettingsDto userSettingsDto);
    }
}
