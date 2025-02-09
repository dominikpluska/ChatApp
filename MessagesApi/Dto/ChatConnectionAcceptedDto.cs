namespace MessagesApi.Dto
{
    public class ChatConnectionAcceptedDto
    {
        public string UserId { get; set; }
        public bool IsAccepted { get; set; } = false;
    }
}
