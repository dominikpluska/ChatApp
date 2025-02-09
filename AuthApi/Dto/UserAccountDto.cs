using AuthApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthApi.Dto
{
    public class UserAccountDto
    {
        public string UserAccountId { get; set; } 
        public  string UserName { get; set; }
        public  string Email { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; } 
        public string PicturePath { get; set; }
    }
}
