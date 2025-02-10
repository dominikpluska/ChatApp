using AuthApi.UserAccessor;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using AuthApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthApi.Services
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

        public async Task<IResult> CreateUserSettings(string userId)
        {
            ArgumentNullException.ThrowIfNull(userId);
            var result = await ApiOperationPost(userId!, $"{_userSettingsApi}/CreateChatsTable");

            return Results.Ok("Operation successful");
        }

        private async Task<object> ApiOperationPost(string userId, string apiPath)
        {
            
            UserSettings userSettings = new UserSettings()
            {
                UserId = userId 
            };
            var jsonContent = JsonSerializer.Serialize(userSettings);
            //var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("MessagesApi");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _userAccessor.TokenString);

            var response = await client.PostAsJsonAsync<string>(apiPath, jsonContent);
            response.EnsureSuccessStatusCode();

            var apiContent = await response.Content.ReadAsStringAsync();
            return apiContent;
        }
    }
}
