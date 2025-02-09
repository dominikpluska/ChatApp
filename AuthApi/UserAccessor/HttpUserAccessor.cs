namespace AuthApi.UserAccessor
{
    public class HttpUserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _accessor;
        public HttpUserAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string TokenString => _accessor.HttpContext!.Request.Cookies["ChatApp"]!;
        public string UserId => _accessor.HttpContext!.Request.Cookies["ChatAppUserId"]!;
        public string UserName => _accessor.HttpContext!.Request.Cookies["ChatAppUserName"]!;

        public void SetCookie(string cookieName, string data, CookieOptions cookieOptions)
        {
            _accessor.HttpContext!.Response.Cookies.Append(cookieName, data, cookieOptions);
        }
    }
}
