using AuthApi.Data;
using AuthApi.Dto;
using AuthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.DataBaseOperations.Commands.RoleCommands
{
    public class RoleCommands : IRoleCommands
    {
        private readonly ApplicationDbContext _context;
        public RoleCommands(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IResult> AddNewRole(string roleName)
        {
            Role role = new Role() { RoleName = roleName };
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return Results.Ok("New Role has been added");
        }

        public async Task<IResult> UpdateRole(RoleDto roleDto)
        {
            await _context.Roles.Where(x => x.RoleId == roleDto.RoleId).ExecuteUpdateAsync(x => x.SetProperty(
                x => x.RoleName, x => roleDto.RoleName
                ));

            return Results.Ok("Record Updated");
        }

        public async Task<IResult> DeleteRole(string roleId)
        {
            await _context.Roles.Where(x => x.RoleId == roleId).ExecuteDeleteAsync();
            return Results.Ok("Record Deleted!");
        }

    }
}
