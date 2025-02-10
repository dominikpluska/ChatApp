namespace AuthApi.Services
{
    public interface IUserSettingsService
    {
        public Task<IResult> CreateUserSettings(string userId);
    }
}
