using MessagesApi.Dto;

namespace MessagesApi.Managers.ChatManager
{
    public interface IChatManager
    {
        public Task<IResult> SendChatRequest(ChatRequestDto chatConnectionDto);
        public Task<IResult> AcceptChatRequest(AcceptChatRequestDto acceptChatRequestDto);
        public Task<IResult> PostMessage(MessageDto message);
        public Task<IResult> GetMessages(string chatId);
        public Task<IResult> GetMessage(string chatId, string messageId);
        public Task<IResult> UpdateChatMessage(MessageUpdateDto messageUpdateDto);
        public Task<IResult> DeleteChatMessage(string chatId, string messageId);
        public Task<IResult> LeaveChat(string chatId);
    }
}
