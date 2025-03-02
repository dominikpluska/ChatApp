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
        public UserAccountsCommands(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddNewUser(UserAccount userAccount)
        {
             //using var dbContext = await _context.CreateDbContextAsync();
             //var user = _mapper.Map<UserAccount>(userRegistration);
             var userFromDb = await _context.UserAccounts.AddAsync(userAccount);
             await _context.SaveChangesAsync();

             return userFromDb.Entity.UserAccountId;
        }

        public async Task<IResult> DeleteUser(string userId)
        {
            //using var dbContext = await _context.CreateDbContextAsync();
            await _context.UserAccounts.Where(x => x.UserAccountId == userId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return Results.Ok("User deleted");
        }

        public async Task<IResult> UpdateUser(UserAccountDto userAccountDto)
        {
            //using var dbContext = await _context.CreateDbContextAsync();
            await _context.UserAccounts.Where(x => x.UserAccountId == userAccountDto.UserAccountId).ExecuteUpdateAsync(
                x => x.SetProperty(x => x.UserName, x => userAccountDto.UserName)
                .SetProperty(x => x.IsActive, x => userAccountDto.IsActive)
                .SetProperty(x => x.Email, x => userAccountDto.Email)
                .SetProperty(x => x.PicturePath, x => userAccountDto.PicturePath)
                );
            await _context.SaveChangesAsync();
            return Results.Ok("Record Updated");
        }

        public async Task<IResult> UpdatePassword(UserPasswordChange userPasswordChange)
        {
            //using var dbContext = await _context.CreateDbContextAsync();
            await _context.UserAccounts.Where(x => x.UserAccountId == userPasswordChange.UserAccountId).ExecuteUpdateAsync(
                x => x.SetProperty(x => x.PasswordHash, x => userPasswordChange.PasswordHash)
               );
            await _context.SaveChangesAsync();
            return Results.Ok("Password Updated");
        }
    }
}
