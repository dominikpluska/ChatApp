using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace MessagesApi.Dto
{
    public class MessageDto
    {
        [JsonPropertyName("ChatId")]
        public required string ChatId { get; set; }
        [JsonPropertyName("TextMessage")]
        public required string TextMessage { get; set; }
    }
}
