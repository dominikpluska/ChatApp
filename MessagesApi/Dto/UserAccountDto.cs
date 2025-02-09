using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MessagesApi.Dto
{
    public class UserAccountDto
    {
        [JsonPropertyName("userAccountId")]
        public string UserAccountId { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("roleName")]
        public string RoleName { get; set; }
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
        [JsonPropertyName("picturePath")]
        public string PicturePath { get; set; }

    }
}
