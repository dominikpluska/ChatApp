using AuthApi.Data;
using AuthApi.Dto;
using AuthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.DataBaseOperations.Commands.RoleCommands
{
    public class RoleCommands : IRoleCommands
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context;
        public RoleCommands(IDbContextFactory<ApplicationDbContext> context)
        {
            _context = context;
        }

        public async Task<IResult> AddNewRole(string roleName)
        {
            using var dbContext = await _context.CreateDbContextAsync();
            Role role = new Role() { RoleName = roleName };
            await dbContext.Roles.AddAsync(role);
            await dbContext.SaveChangesAsync();
            return Results.Ok("New Role has been added");
        }

        public async Task<IResult> UpdateRole(RoleDto roleDto)
        {
            using var dbContext = await _context.CreateDbContextAsync();
            await dbContext.Roles.Where(x => x.RoleId == roleDto.RoleId).ExecuteUpdateAsync(x => x.SetProperty(
                x => x.RoleName, x => roleDto.RoleName
                ));

            return Results.Ok("Record Updated");
        }

        public async Task<IResult> DeleteRole(string roleId)
        {
            using var dbContext = await _context.CreateDbContextAsync();
            await dbContext.Roles.Where(x => x.RoleId == roleId).ExecuteDeleteAsync();
            return Results.Ok("Record Deleted!");
        }

    }
}
