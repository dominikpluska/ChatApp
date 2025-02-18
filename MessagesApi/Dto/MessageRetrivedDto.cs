namespace MessagesApi.Dto
{
    public class MessageRetrivedDto
    {
        public string MessageId { get; set; }
        public string TextMessage { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
