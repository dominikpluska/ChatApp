using AuthApi.Data;
using AuthApi.Dto;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.DataBaseOperations.Repository.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public RoleRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RoleDto>> GetRoles()
        {
            var rolesList = await _context.Roles.ToListAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(rolesList);
        }

        public async Task<RoleDto> GetRoleById(string roleId)
        {
            var role = await _context.Roles.Where(x => x.RoleId == roleId).FirstOrDefaultAsync();
            return _mapper.Map<RoleDto>(role);
        }
        public async Task<RoleDto> GetRoleByName(string roleName)
        {
            var role = await _context.Roles.Where(x => x.RoleName == roleName).FirstOrDefaultAsync();
            return _mapper.Map<RoleDto>(role);
        }

    }
}
