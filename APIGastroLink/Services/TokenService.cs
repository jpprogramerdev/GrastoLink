using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIGastroLink.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace APIGastroLink.Services {
    public class TokenService : ITokenService{
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config) {
            _config = config;
        }

        public string GenerateToken(string userId, string userName, IEnumerable<string> roles = null) {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            var key = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expireMinutes = int.Parse(_config["Jwt:ExpireMinutes"] ?? "60");


            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName)
            };

            if (roles != null) {
                foreach (var role in roles) {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
