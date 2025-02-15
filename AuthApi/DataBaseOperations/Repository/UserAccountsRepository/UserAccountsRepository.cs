using AuthApi.Data;
using AuthApi.Dto;
using AuthApi.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.DataBaseOperations.Repository.UserAccountsRepository
{
    public class UserAccountsRepository : IUserAccountsRepository
    {
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

        public async Task<IEnumerable<UserAccountDto>> GetAllActiveUsers()
        {
            var result = await _context.UserAccounts.Include(role => role.Role).Where(x => x.IsActive == true).ToListAsync();

            return _mapper.Map<IEnumerable<UserAccountDto>>(result);
        }

        public async Task<IEnumerable<UserAccount>> GetTop100ActiveUsersOrderedAlphabetically()
        {
            IQueryable<UserAccount> result = _context.UserAccounts.Include(role => role.Role)
                                            .Where(x => x.IsActive == true)
                                            .OrderBy(x => x.UserName).Take(100);

            return await result.ToListAsync();
        }
    }
}
