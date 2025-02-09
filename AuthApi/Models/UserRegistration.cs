namespace AuthApi.Models
{
    public class UserRegistration
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string RoleId { get; set; }
        public string ProfilePicturePath { get; set; }
    }
}
