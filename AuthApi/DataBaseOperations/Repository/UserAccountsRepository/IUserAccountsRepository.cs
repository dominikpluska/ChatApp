using AuthApi.Dto;
using AuthApi.Models;

namespace AuthApi.DataBaseOperations.Repository.UserAccountsRepository
{
    public interface IUserAccountsRepository
    {
        public Task<IEnumerable<UserAccountDto>> GetAllUsers();
        public Task<UserAccount> GetUser(string userId);
        public Task<UserAccount> GetUserByName(string userName);
        public Task<UserAccount> GetUserByEmail(string email);
        public Task<IEnumerable<UserAccountDto>> GetAllActiveUsers();
        public Task<IEnumerable<UserAccount>> GetTop100ActiveUsersOrderedAlphabetically();
    }
}
