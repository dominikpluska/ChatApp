using MessagesApi.Dto;

namespace MessagesApi.Managers.ChatManager
{
    public interface IChatManager
    {
        public Task<IResult> PostMessage(MessageDto message, CancellationToken cancellationToken);
        public Task<IResult> GetMessages(string chatId, CancellationToken cancellationToken);
        public Task<IResult> GetMessage(string chatId, string messageId, CancellationToken cancellationToken);
        public Task<IResult> UpdateChatMessage(MessageUpdateDto messageUpdateDto, CancellationToken cancellationToken);
        public Task<IResult> DeleteChatMessage(string chatId, string messageId, CancellationToken cancellationToken);
        public Task<IResult> LeaveChat(string chatId, CancellationToken cancellationToken);
        public Task<IResult> OpenChat(string chatterId, CancellationToken cancellationToken);
    }
}
