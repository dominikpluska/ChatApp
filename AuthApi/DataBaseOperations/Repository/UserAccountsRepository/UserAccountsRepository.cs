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
        private readonly IDbContextFactory<ApplicationDbContext> _context;
        private readonly IMapper _mapper;
        public UserAccountsRepository(IDbContextFactory<ApplicationDbContext> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserAccountDto>> GetAllUsers()
        {
            using var dbContext = await _context.CreateDbContextAsync();
            var result = await dbContext.UserAccounts.Include(role => role.Role).ToListAsync();

            return _mapper.Map<IEnumerable<UserAccountDto>>(result);
        }

        public async Task<UserAccount> GetUser(string userId)
        {
            using var dbContext = await _context.CreateDbContextAsync();
            var result = await dbContext.UserAccounts.Include(role => role.Role).Where(x => x.UserAccountId == userId).FirstOrDefaultAsync();

            return _mapper.Map<UserAccount>(result);
        }

        public async Task<UserAccount> GetUserByName(string userName)
        {
            using var dbContext = await _context.CreateDbContextAsync();
            var result = await dbContext.UserAccounts.Include(role => role.Role).Where(x => x.UserName == userName).FirstOrDefaultAsync();

            return _mapper.Map<UserAccount>(result);
        }

        public async Task<UserAccount> GetUserByEmail(string email)
        {
            using var dbContext = await _context.CreateDbContextAsync();
            var result = await dbContext.UserAccounts.Include(role => role.Role).Where(x => x.Email == email).FirstOrDefaultAsync();

            return _mapper.Map<UserAccount>(result);
        }

        public async Task<IEnumerable<UserAccount>> SelectUsersFromIdList(IEnumerable<string> listOfIds)
        {
            try
            {
                using var dbContext = await _context.CreateDbContextAsync();
                var results = await dbContext.UserAccounts.Where(x => listOfIds.Contains(x.UserAccountId)).ToListAsync();

                return results;
            }
            catch(ArgumentNullException ex)
            {
                throw new ArgumentNullException("Argument null exception!", ex.Message);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<UserAccountDto>> GetAllActiveUsers()
        {
            using var dbContext = await _context.CreateDbContextAsync();
            var result = await dbContext.UserAccounts.Include(role => role.Role).Where(x => x.IsActive == true).ToListAsync();

            return _mapper.Map<IEnumerable<UserAccountDto>>(result);
        }

        public async Task<int> GetActiveUsersCount()
        {
            try
            {
                using var dbContext = await _context.CreateDbContextAsync();
                var userCount = await dbContext.UserAccounts.Where(x => x.IsActive == true).CountAsync();
                return userCount;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<UserAccount>> GetTopActiveUsersOrderedAlphabetically(int itemsToSkip)
        {
            try
            {
                using var dbContext = await _context.CreateDbContextAsync();
                IQueryable<UserAccount> result = dbContext.UserAccounts.Include(role => role.Role)
                                .Where(x => x.IsActive == true)
                                .OrderBy(x => x.UserName).Skip(itemsToSkip).Take(20);

                return await result.ToListAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<UserAccount>> Search(string userName)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userName);

                using var dbContext = await _context.CreateDbContextAsync();
                var results = dbContext.UserAccounts.Where(x => x.UserName.ToLower().Contains(userName.ToLower())).OrderDescending();

                return await results.ToListAsync();
            }
            catch(ArgumentNullException ex)
            {
                throw new ArgumentNullException("Argument null exception!", ex.Message);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
