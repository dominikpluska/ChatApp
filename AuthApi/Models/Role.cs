using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models
{
    public class Role   
    {
        [Key]
        [Required]
        public string RoleId { get; set; } = Guid.NewGuid().ToString(); 
        [Required]
        [StringLength(100)]
        public required string RoleName { get; set; }
        public ICollection<UserAccount> UserAccount { get; } = new List<UserAccount>();
    }
}
