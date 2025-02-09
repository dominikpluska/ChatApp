namespace AuthApi.Jwt
{
    public interface IJwt
    {
        public string GenerateToken(string userName, string role);
    }
}
