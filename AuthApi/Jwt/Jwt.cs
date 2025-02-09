using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthApi.Jwt
{
    public class Jwt : IJwt
    {
        private readonly string _tokenString;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IConfiguration _configuration;

        public Jwt(IConfiguration configuration)
        {
            _configuration = configuration;
            _tokenString = _configuration.GetValue<string>("JwtSettings:TokenString")!;
            _issuer = _configuration.GetValue<string>("JwtSettings:Issuer")!;
            _audience = _configuration.GetValue<string>("JwtSettings:Audience")!;
        }

        public string GenerateToken(string userName, string role)
        {
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.Name, userName, ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenString));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
