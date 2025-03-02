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

        public async Task<IEnumerable<UserAccountDto>> GetAllUsers()
        {
            var result = await _context.UserAccounts.Include(role => role.Role).ToListAsync();

            return _mapper.Map<IEnumerable<UserAccountDto>>(result);
        }

        public async Task<UserAccount> GetUser(string userId)
        {
            
            var result = await _context.UserAccounts.Include(role => role.Role).Where(x => x.UserAccountId == userId).FirstOrDefaultAsync();

            return _mapper.Map<UserAccount>(result);
        }

        public async Task<UserAccount> GetUserByName(string userName)
        {
            
            var result = await _context.UserAccounts.Include(role => role.Role).Where(x => x.UserName == userName).FirstOrDefaultAsync();

            return _mapper.Map<UserAccount>(result);
        }

        public async Task<UserAccount> GetUserByEmail(string email)
        {
            
            var result = await _context.UserAccounts.Include(role => role.Role).Where(x => x.Email == email).FirstOrDefaultAsync();

            return _mapper.Map<UserAccount>(result);
        }

        public async Task<IEnumerable<UserAccount>> SelectUsersFromIdList(IEnumerable<string> listOfIds)
        {

           
            var results = await _context.UserAccounts.Where(x => listOfIds.Contains(x.UserAccountId)).ToListAsync();

            return results;
        }

        public async Task<IEnumerable<UserAccountDto>> GetAllActiveUsers()
        {
            
            var result = await _context.UserAccounts.Include(role => role.Role).Where(x => x.IsActive == true).ToListAsync();

            return _mapper.Map<IEnumerable<UserAccountDto>>(result);
        }

        public async Task<int> GetActiveUsersCount()
        {

            
            var userCount = await _context.UserAccounts.Where(x => x.IsActive == true).CountAsync();
            return userCount;

        }

        public async Task<IEnumerable<UserAccount>> GetTopActiveUsersOrderedAlphabetically(int itemsToSkip)
        {
                
                IQueryable<UserAccount> result = _context.UserAccounts.Include(role => role.Role)
                                .Where(x => x.IsActive == true)
                                .OrderBy(x => x.UserName).Skip(itemsToSkip).Take(20);

                return await result.ToListAsync();
        }

        public async Task<IEnumerable<UserAccount>> Search(string userName)
        {
           ArgumentNullException.ThrowIfNull(userName);

           
            var results = _context.UserAccounts.Where(x => x.UserName.ToLower().Contains(userName.ToLower())).OrderDescending();

            return await results.ToListAsync();
        }
    }
}
