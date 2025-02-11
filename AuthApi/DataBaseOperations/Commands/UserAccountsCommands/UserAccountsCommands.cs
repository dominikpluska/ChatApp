using AuthApi.Data;
using AuthApi.Dto;
using AuthApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.DataBaseOperations.Commands.UserAccountsCommands
{
    public class UserAccountsCommands : IUserAccountsCommands
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;   
        public UserAccountsCommands(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> AddNewUser(UserRegistration userRegistration)
        {
            try
            {
                var user = _mapper.Map<UserAccount>(userRegistration);
                var userFromDb = await _context.UserAccounts.AddAsync(user);
                await _context.SaveChangesAsync();

                return userFromDb.Entity.UserAccountId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IResult> DeleteUser(string userId)
        {
            await _context.UserAccounts.Where(x => x.UserAccountId == userId).ExecuteDeleteAsync();
            return Results.Ok("User deleted");
        }

        public async Task<IResult> UpdateUser(UserAccountDto userAccountDto)
        {
            await _context.UserAccounts.Where(x => x.UserAccountId == userAccountDto.UserAccountId).ExecuteUpdateAsync(
                x => x.SetProperty(x => x.UserName, x => userAccountDto.UserName)
                .SetProperty(x => x.IsActive, x => userAccountDto.IsActive)
                .SetProperty(x => x.Email, x => userAccountDto.Email)
                .SetProperty(x => x.PicturePath, x => userAccountDto.PicturePath)
                );

            return Results.Ok("Record Updated");
        }

        public async Task<IResult> UpdatePassword(UserPasswordChange userPasswordChange)
        {
            await _context.UserAccounts.Where(x => x.UserAccountId == userPasswordChange.UserAccountId).ExecuteUpdateAsync(
                x => x.SetProperty(x => x.PasswordHash, x => userPasswordChange.PasswordHash)
               );

            return Results.Ok("Password Updated");
        }
    }
}
