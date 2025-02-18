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
            var client = CreateHttpClient();
            var response = await client.GetAsync($"{_authApi}/AuthCheck");
        }

        public async Task<string> GetUser(string userName)
        {
            ArgumentNullException.ThrowIfNull(userName);

            var client = CreateHttpClient();
            var response = await client.GetAsync($"{_authApi}/GetUser?userName={userName}");
            var apiContent = await ReadApiContent(response);

            var deserializedContent = JsonSerializer.Deserialize<string>(apiContent);

            return deserializedContent!;
        }

        public async Task<IEnumerable<UserAccountLightDto>> GetUserListByIds(IdRequestsDto idRequestsDto)
        {
            ArgumentNullException.ThrowIfNull(idRequestsDto);

            var client = CreateHttpClient();
            var jsonContent = JsonSerializer.Serialize(idRequestsDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_authApi}/GetUserListByIds", content);
            var apiContent = await ReadApiContent(response);

            var deserializedContent = JsonSerializer.Deserialize<IEnumerable<UserAccountLightDto>>(apiContent);

            return deserializedContent!;
        }

        public async Task<UserAccountDto> GetAccountProperties(string userId)
        {
            ArgumentNullException.ThrowIfNull(userId);
            var client = CreateHttpClient();

            var response = await client.GetAsync($"{_authApi}/GetAccountProperties?userId={userId}");
            var apiContent = await ReadApiContent(response);


            var userAccountDto = JsonSerializer.Deserialize<UserAccountDto>(apiContent.ToString()!);

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
            var client = _httpClientFactory.CreateClient("Auth");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _userAccessor.TokenString);

            return client;

        }
    }
}
