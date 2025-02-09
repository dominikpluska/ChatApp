using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.ChatsRepository
{
    public interface IChatsRepository
    {
        public Task<Chats> GetChatList(string userId);
    }
}
