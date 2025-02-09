using MongoDB.Bson;

namespace MessagesApi.Dto
{
    public class MessageUpdateDto
    {
        public required string ChatId { get; set; }
        public required string MessageId { get; set; }
        public required string TextMessage { get; set; }
    }
}
