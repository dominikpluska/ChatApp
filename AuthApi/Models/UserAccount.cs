using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace AuthApi.Models
{
    public class UserAccount
    {
        [Key]
        [Required]
        public string UserAccountId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        [ForeignKey("RoleId")]
        public required string RoleId { get; set; }

        public Role? Role { get; set; }
        public bool IsActive { get; set; } = true;
        public string? PicturePath { get; set; }
    }
}
