namespace AuthApi.UserAccessor
{
    public interface IUserAccessor
    {
        string UserName { get; }
        string TokenString { get; }
        public string UserId { get; }

        void SetCookie(string cookieName, string data, CookieOptions cookieOptions);
    }
}
