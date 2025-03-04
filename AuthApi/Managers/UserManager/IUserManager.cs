using AuthApi.Dto;

namespace AuthApi.Managers.UserManager
{
    public interface IUserManager
    {
        public Task<IResult> RegisterNewUser(UserRegistrationDto userDto, CancellationToken cancellationToken);
        public Task<IResult> Login(UserLoginDto userLoginDto, CancellationToken cancellationToken);
        public Task<IResult> LogOut(CancellationToken cancellationToken);
        public Task<IResult> UpdateAccount(UserAccountUpdateDto userDto, CancellationToken cancellationToken);
        public Task<IResult> CheckAuthentication(CancellationToken cancellationToken);
        public Task<IResult> GetAccountProperties(string userId, CancellationToken cancellationToken);
        public Task<IResult> GetActiveUserList(CancellationToken cancellationToken, int itemsToSkip = 0);
        public Task<IResult> SearchForUser(string userName, CancellationToken cancellationToken);
        public Task<IResult> PostUserListByIds(IEnumerable<string> Ids, CancellationToken cancellationToken);

        public Task<IResult> UpdatePassword(NewPasswordDto newPasswordDto, CancellationToken cancellationToken);
    }
}
