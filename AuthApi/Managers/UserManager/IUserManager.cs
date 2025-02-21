using AuthApi.Dto;

namespace AuthApi.Managers.UserManager
{
    public interface IUserManager
    {
        public Task<IResult> RegisterNewUser(UserRegistrationDto userDto);
        public Task<IResult> Login(UserLoginDto userLoginDto);
        public Task<IResult> LogOut();
        public Task<IResult> UpdateAccount(UserAccountUpdateDto userDto);
        public Task<IResult> CheckAuthentication();
        public Task<IResult> GetAccountProperties(string userId);
        public Task<IResult> GetActiveUserList(int itemsToSkip = 0);
        public Task<IResult> SearchForUser(string userName);
        public Task<IResult> PostUserListByIds(IEnumerable<string> Ids);

        public Task<IResult> UpdatePassword(NewPasswordDto newPasswordDto);
    }
}
