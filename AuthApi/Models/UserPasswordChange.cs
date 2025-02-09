namespace AuthApi.Models
{
    public class UserPasswordChange
    {
        public string UserAccountId { get; set; }
        public string PasswordHash { get; set; }
    }
}
