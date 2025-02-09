using MessagesApi.Dto;

namespace MessagesApi.Services
{
    public interface IAuthenticationService
    {
        public Task Authorize(HttpContext httpContext);
        public Task<string> GetUser(string userName);

        public Task<UserAccountDto> GetAccountProperties(string userId);
    }
}
