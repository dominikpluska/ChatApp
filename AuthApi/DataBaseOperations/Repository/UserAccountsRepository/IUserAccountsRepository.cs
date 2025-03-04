using AuthApi.Dto;
using AuthApi.Models;

namespace AuthApi.DataBaseOperations.Repository.UserAccountsRepository
{
    public interface IUserAccountsRepository
    {
        public Task<IEnumerable<UserAccountDto>> GetAllUsers(CancellationToken cacncellationToken);
        public Task<UserAccount> GetUser(string userId, CancellationToken cacncellationToken);
        public Task<UserAccount> GetUserByName(string userName, CancellationToken cacncellationToken);
        public Task<UserAccount> GetUserByEmail(string email, CancellationToken cacncellationToken);
        public Task<IEnumerable<UserAccountDto>> GetAllActiveUsers(CancellationToken cacncellationToken);
        public Task<int> GetActiveUsersCount(CancellationToken cacncellationToken);
        public Task<IEnumerable<UserAccount>> GetTopActiveUsersOrderedAlphabetically(int itemsToSkip, CancellationToken cacncellationToken);
        public Task<IEnumerable<UserAccount>> Search(string userName, CancellationToken cacncellationToken);
        public Task<IEnumerable<UserAccount>> SelectUsersFromIdList(IEnumerable<string> listOfIds, CancellationToken cacncellationToken);
    }
}
