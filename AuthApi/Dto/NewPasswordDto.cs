using System.Text.Json.Serialization;

namespace AuthApi.Dto
{
    public class NewPasswordDto
    {
        [JsonPropertyName("currentPassword")]
        public required string CurrentPassword { get; set; }
        [JsonPropertyName("newPassword")]
        public required string NewPassword { get; set; }
        [JsonPropertyName("confirmPassword")]
        public required string ConfirmPassword { get; set; }
    }
}
