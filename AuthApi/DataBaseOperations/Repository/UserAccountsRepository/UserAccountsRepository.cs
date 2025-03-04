using AuthApi.Data;
using AuthApi.Dto;
using AuthApi.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace AuthApi.DataBaseOperations.Repository.UserAccountsRepository
{
    public class UserAccountsRepository : IUserAccountsRepository
    {
        //private readonly IDbContextFactory<ApplicationDbContext> _context;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UserAccountsRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserAccountDto>> GetAllUsers(CancellationToken cacncellationToken)
        {
            var result = await _context.UserAccounts.Include(role => role.Role).ToListAsync(cacncellationToken);

            return _mapper.Map<IEnumerable<UserAccountDto>>(result);
        }

        public async Task<UserAccount> GetUser(string userId, CancellationToken cacncellationToken)
        {
            
            var result = await _context.UserAccounts.Include(role => role.Role).Where(x => x.UserAccountId == userId).FirstOrDefaultAsync(cacncellationToken);

            return _mapper.Map<UserAccount>(result);
        }

        public async Task<UserAccount> GetUserByName(string userName, CancellationToken cacncellationToken)
        {
            
            var result = await _context.UserAccounts.Include(role => role.Role).Where(x => x.UserName == userName).FirstOrDefaultAsync(cacncellationToken);

            return _mapper.Map<UserAccount>(result);
        }

        public async Task<UserAccount> GetUserByEmail(string email, CancellationToken cacncellationToken)
        {
            
            var result = await _context.UserAccounts.Include(role => role.Role).Where(x => x.Email == email).FirstOrDefaultAsync(cacncellationToken);

            return _mapper.Map<UserAccount>(result);
        }

        public async Task<IEnumerable<UserAccount>> SelectUsersFromIdList(IEnumerable<string> listOfIds, CancellationToken cacncellationToken)
        {

           
            var results = await _context.UserAccounts.Where(x => listOfIds.Contains(x.UserAccountId)).ToListAsync(cacncellationToken);

            return results;
        }

        public async Task<IEnumerable<UserAccountDto>> GetAllActiveUsers(CancellationToken cacncellationToken)
        {
            
            var result = await _context.UserAccounts.Include(role => role.Role).Where(x => x.IsActive == true).ToListAsync(cacncellationToken);

            return _mapper.Map<IEnumerable<UserAccountDto>>(result);
        }

        public async Task<int> GetActiveUsersCount(CancellationToken cacncellationToken)
        {

            
            var userCount = await _context.UserAccounts.Where(x => x.IsActive == true).CountAsync(cacncellationToken);
            return userCount;

        }

        public async Task<IEnumerable<UserAccount>> GetTopActiveUsersOrderedAlphabetically(int itemsToSkip, CancellationToken cacncellationToken)
        {
                
                IQueryable<UserAccount> result = _context.UserAccounts.Include(role => role.Role)
                                .Where(x => x.IsActive == true)
                                .OrderBy(x => x.UserName).Skip(itemsToSkip).Take(20);

                return await result.ToListAsync(cacncellationToken);
        }

        public async Task<IEnumerable<UserAccount>> Search(string userName, CancellationToken cacncellationToken)
        {
           ArgumentNullException.ThrowIfNull(userName);

           
            var results = _context.UserAccounts.Where(x => x.UserName.ToLower().Contains(userName.ToLower())).OrderDescending();

            return await results.ToListAsync(cacncellationToken);
        }
    }
}
