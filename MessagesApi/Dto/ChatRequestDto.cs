namespace MessagesApi.Dto
{
    public class ChatRequestDto
    {
        public required string RequestInitializer { get; set; }
        public required List<string> RequestRecipient { get; set; }
    }
}
