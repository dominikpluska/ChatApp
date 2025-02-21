using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MessagesApi.UserAccessor;

namespace MessagesApi.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserAccessor _userAccessor;
        private readonly IConfiguration _configuration;
        private readonly string _userSettingsApi;

        public UserSettingsService(IHttpClientFactory httpClientFactory, IUserAccessor userAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _userAccessor = userAccessor;
            _configuration = configuration;
            _userSettingsApi = _configuration.GetValue<string>("Apis:UserSettingsApi")!;

        }

        public async Task<string> CheckIfUserIsBlocked(string userId, string chatterId)
        {
            ArgumentNullException.ThrowIfNull(userId);
            var client = CreateHttpClient();

            var response = await client.GetAsync($"{_userSettingsApi}/GetUserFromBlackList/u={userId}/c={chatterId}");
            var apiContent = await ReadApiContent(response);

            if(apiContent == string.Empty)
            {
                return null;
            }

            var userAccountDto = JsonSerializer.Deserialize<string>(apiContent.ToString()!);

            return userAccountDto!;
        }

        private static async Task<string> ReadApiContent(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var apiContent = await response.Content.ReadAsStringAsync();

            return apiContent;
        }

        private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient("UserSettings");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _userAccessor.TokenString);

            return client;

        }
    }
}
