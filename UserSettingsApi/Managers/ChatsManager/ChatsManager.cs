using UserSettingsApi.DatabaseOperations.Commands.ChatsCommands;
using UserSettingsApi.DatabaseOperations.Repository.ChatsRepository;
using UserSettingsApi.Models;
using UserSettingsApi.UserAccessor;

namespace UserSettingsApi.Managers.ChatsManager
{
    public class ChatsManager : IChatsManager
    {
        private readonly IChatsRepository _chatsRepository;
        private readonly IChatsCommands _chatsCommands;
        private readonly IUserAccessor _userAccessor;

        public ChatsManager(IChatsRepository chatsRepository, IUserAccessor userAccessor, IChatsCommands chatsCommands)
        {
            _chatsRepository = chatsRepository;
            _userAccessor = userAccessor;
            _chatsCommands = chatsCommands;
        }

        public async Task<IResult> GetAllChats()
        {
            try
            {
                var userId = _userAccessor.UserId;
                var chats = await _chatsRepository.GetChatList(userId);
                return Results.Ok(chats.ChatList);
            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> CreateChatsTable(string userId)
        {
            try
            {
                Chat chat = new Chat()
                {
                    UserId = userId,
                };

                await _chatsCommands.CreateChatsTable(chat);
                return Results.Ok("Chats table created");
            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
