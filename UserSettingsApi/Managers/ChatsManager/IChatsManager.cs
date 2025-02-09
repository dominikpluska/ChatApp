namespace UserSettingsApi.Managers.ChatsManager
{
    public interface IChatsManager
    {
        public Task<IResult> GetAllChats();
    }
}
