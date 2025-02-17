using UserSettingsApi.Dto;

namespace UserSettingsApi.Services
{
    public interface IAuthenticationService
    {
        public Task Authorize(HttpContext httpContext);
        public Task<string> GetUser(string userName);

        public Task<UserAccountDto> GetAccountProperties(string userId);

        public Task<IEnumerable<UserAccountLightDto>> GetUserListByIds(IdRequestsDto idRequestsDto);
    }
}
