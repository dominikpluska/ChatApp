namespace AuthApi.Dto
{
    public class UserPasswordChangeDto
    {
        public required string UserAccountId { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
        public required string NewPasswordConfirmed { get; set; }
    }
}
