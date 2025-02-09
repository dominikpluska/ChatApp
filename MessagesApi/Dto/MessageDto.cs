using MongoDB.Bson;

namespace MessagesApi.Dto
{
    public class MessageDto
    {
        public required string ChatId { get; set; }
        public required string TextMessage { get; set; }
    }
}
