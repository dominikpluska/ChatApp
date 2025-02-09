namespace UserSettingsApi.UserAccessor
{
    public interface IUserAccessor
    {
        string UserId { get; }
        string UserName { get; }
        string TokenString { get; }

        void SetCookie(string cookieName, string data, CookieOptions cookieOptions);
    }
}
