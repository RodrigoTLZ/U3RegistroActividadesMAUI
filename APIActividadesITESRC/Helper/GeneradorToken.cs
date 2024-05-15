using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIActividadesITESRC.Helper
{
    public class GeneradorToken
    {
        private readonly IConfiguration configuration;

        public GeneradorToken(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetToken(string username, string role, List<Claim> claims)
        {
            JwtSecurityTokenHandler handler = new();

            var issuer = configuration.GetSection("Jwt").GetValue<string>("Issuer") ?? "";
            var audience = configuration.GetSection("Jwt").GetValue<string>("Audience") ?? "";
            var secret = configuration.GetSection("Jwt").GetValue<string>("Secret") ?? "";

            List<Claim> listadoclaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Iss, issuer),
                new Claim(JwtRegisteredClaimNames.Aud, audience),
            };

            JwtSecurityToken jwtSecurity = new(
                issuer,
                audience,
                listadoclaims,
                DateTime.Now,
                DateTime.Now.AddMinutes(20),
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret ?? "")),
                SecurityAlgorithms.HmacSha256)
                );

            return handler.WriteToken(jwtSecurity);


        }
    }
}

