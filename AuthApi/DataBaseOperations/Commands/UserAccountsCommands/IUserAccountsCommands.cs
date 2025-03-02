using AuthApi.Dto;
using AuthApi.Models;

namespace AuthApi.DataBaseOperations.Commands.UserAccountsCommands
{
    public interface IUserAccountsCommands
    {
        public Task<string> AddNewUser(UserAccount userAccount);
        public Task<IResult> DeleteUser(string userId);
        public Task<IResult> UpdateUser(UserAccountDto userAccountDto);
        public Task<IResult> UpdatePassword(UserPasswordChange userPasswordChangeDto);

    }
}
