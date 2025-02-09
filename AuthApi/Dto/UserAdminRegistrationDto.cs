namespace AuthApi.Dto
{
    public class UserAdminRegistrationDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string Role { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}
