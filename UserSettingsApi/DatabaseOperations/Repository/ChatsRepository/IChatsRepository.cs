using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.ChatsRepository
{
    public interface IChatsRepository
    {
        public Task<Chat> GetChatList(string userId, CancellationToken cancellationToken);
    }
}
