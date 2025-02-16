using AuthApi.Data;
using AuthApi.Dto;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.DataBaseOperations.Repository.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context;
        private readonly IMapper _mapper;
        public RoleRepository(IDbContextFactory<ApplicationDbContext> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RoleDto>> GetRoles()
        {
            using var dbContext = await _context.CreateDbContextAsync();
            var rolesList = await dbContext.Roles.ToListAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(rolesList);
        }

        public async Task<RoleDto> GetRoleById(string roleId)
        {
            using var dbContext = await _context.CreateDbContextAsync();
            var role = await dbContext.Roles.Where(x => x.RoleId == roleId).FirstOrDefaultAsync();
            return _mapper.Map<RoleDto>(role);
        }
        public async Task<RoleDto> GetRoleByName(string roleName)
        {
            using var dbContext = await _context.CreateDbContextAsync();
            var role = await dbContext.Roles.Where(x => x.RoleName == roleName).FirstOrDefaultAsync();
            return _mapper.Map<RoleDto>(role);
        }

    }
}
