using UserSettingsApi.DatabaseOperations.Repository.ChatsRepository;
using UserSettingsApi.UserAccessor;

namespace UserSettingsApi.Managers.ChatsManager
{
    public class ChatsManager : IChatsManager
    {
        private readonly IChatsRepository _chatsRepository;
        private readonly IUserAccessor _userAccessor;

        public ChatsManager(IChatsRepository chatsRepository, IUserAccessor userAccessor)
        {
            _chatsRepository = chatsRepository;
            _userAccessor = userAccessor;
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
    }
}
