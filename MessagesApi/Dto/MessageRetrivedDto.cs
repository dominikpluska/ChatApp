namespace MessagesApi.Dto
{
    public class MessageRetrivedDto
    {
        public required string ChatId { get; set; }
        public required string TextMessage { get; set; }
        public string UserId { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
