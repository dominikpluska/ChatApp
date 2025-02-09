using AuthApi.UserAccessor;

namespace AuthApi.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserAccessor _userAccessor;

        public UserSettingsService(IHttpClientFactory httpClientFactory, IUserAccessor userAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _userAccessor = userAccessor;
        }


    }
}
