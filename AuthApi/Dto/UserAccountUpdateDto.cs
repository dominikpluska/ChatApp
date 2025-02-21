namespace AuthApi.Dto
{
    public class UserAccountUpdateDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        //public byte[] ProfilePicture { get; set; }
    }
}
