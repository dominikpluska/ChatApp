namespace AuthApi.Cookies
{
    public interface ICookieGenerator
    {
        public CookieOptions GenerateCookie(DateTime dateTime);
    }
}
