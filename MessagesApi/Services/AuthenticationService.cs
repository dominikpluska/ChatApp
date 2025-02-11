using System.Text.Json;
using System.Text;
using MessagesApi.UserAccessor;
using MessagesApi.Dto;
using Microsoft.Extensions.Configuration;

namespace MessagesApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserAccessor _userAccessor;
        private readonly IConfiguration _configuration;
        private readonly string _authApi;
        public AuthenticationService(IHttpClientFactory httpClientFactory, IUserAccessor userAccessor, IConfiguration configuration)
        {
            _userAccessor = userAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _authApi = _configuration.GetValue<string>("Apis:AuthApi")!;
        }

        public async Task Authorize(HttpContext httpContext)
        {
            var result = await ApiOperation(httpContext, $"{_authApi}/AuthCheck");
        }

        public async Task<string> GetUser(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("UserName was null!");
            }
            var result = await ApiOperation(userName!, $"/GetUser?userName={userName}");
            return result.ToString()!;
        }

        public async Task<UserAccountDto> GetAccountProperties(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("UserId was null!");
            }
            var result = await ApiOperation(userId!, $"{_authApi}/GetAccountProperties?userId={userId}");

            var userAccountDto = JsonSerializer.Deserialize<UserAccountDto>(result.ToString()!);

            return userAccountDto!;
        }

        private async Task<object> ApiOperation(object data, string apiPath)
        {
            var jsonLog = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonLog, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("Auth");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _userAccessor.TokenString);

            var response = await client.GetAsync(apiPath);
            response.EnsureSuccessStatusCode();

            var apiContent = await response.Content.ReadAsStringAsync();
            return apiContent;
        }
    }
}
