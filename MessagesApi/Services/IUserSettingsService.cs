namespace MessagesApi.Services
{
    public interface IUserSettingsService
    {
        public  Task<string> CheckIfUserIsBlocked(string userId, string chatterId);
    }
}
