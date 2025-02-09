namespace MessagesApi.Models
{
    public class ChatParticipant
    {
        public required string UserId { get; set; }
        public bool IsAccepted { get; set; } = false;
    }
}
