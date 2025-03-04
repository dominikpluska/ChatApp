using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Commands.ChatsCommands
{
    public interface IChatsCommands
    {
        public Task<IResult> CreateChatsTable(Chat chat, CancellationToken cancellationToken);
    }
}
