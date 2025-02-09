using MongoDB.Bson;

namespace MessagesApi.Dto
{
    public class AcceptChatRequestDto
    {
        public required string ChatId { get; set; }
    }
}
