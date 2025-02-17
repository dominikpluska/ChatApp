using System.Text.Json.Serialization;

namespace UserSettingsApi.Dto
{
    public class UserAccountLightDto
    {
        [JsonPropertyName("userAccountId")]
        public string UserAccountId { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
    }
}
