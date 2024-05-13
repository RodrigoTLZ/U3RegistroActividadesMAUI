using System.Security.Claims;

namespace APIActividadesITESRC.Helper
{
    public class GeneradorToken
    {
        public string GetToken(string token)
        {
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));

        }
    }
}
