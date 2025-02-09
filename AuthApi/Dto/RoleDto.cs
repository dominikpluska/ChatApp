using AuthApi.Models;
using System.ComponentModel.DataAnnotations;

namespace AuthApi.Dto
{
    public class RoleDto
    {
        public required string RoleId { get; set; }
        public required string RoleName { get; set; }
    }
}
