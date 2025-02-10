using UserSettingsApi.DatabaseOperations.Commands.ChatsCommands;
using UserSettingsApi.DatabaseOperations.Repository.ChatsRepository;
using UserSettingsApi.Dto;
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
                return Results.Ok(chats);
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> CreateChatsTable(UserSettingsDto userSettingsDto)
        {
            try
            {
                Chat chat = new Chat()
                {
                    UserId = userSettingsDto.UserId,
                };

                await _chatsCommands.CreateChatsTable(chat);
                return Results.Ok("Chats table created");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
