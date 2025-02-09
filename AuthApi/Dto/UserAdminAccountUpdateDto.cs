namespace AuthApi.Dto
{
    public class UserAdminAccountUpdateDto
    {
        public required string UserAccountId { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}
