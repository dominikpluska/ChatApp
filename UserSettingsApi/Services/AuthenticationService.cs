using System.Text.Json;
using System.Text;
using UserSettingsApi.Dto;
using UserSettingsApi.UserAccessor;


namespace UserSettingsApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserAccessor _userAccessor;
        public AuthenticationService(IHttpClientFactory httpClientFactory, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Authorize(HttpContext httpContext)
        {
            var result = await ApiOperation(httpContext, "/AuthCheck");
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
            var result = await ApiOperation(userId!, $"/GetUser?userId={userId}");

            return (UserAccountDto)result;
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
