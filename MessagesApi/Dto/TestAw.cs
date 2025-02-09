using MongoDB.Bson;

namespace MessagesApi.Dto
{
    public class TestAw
    {
        public string _id { get; set; } 
        public required string UserId { get; set; }
        public required string TextMessage { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
