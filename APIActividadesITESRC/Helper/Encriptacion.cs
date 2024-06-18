using System.Security.Cryptography;
using System.Text;

namespace APIActividadesITESRC.Helper
{
    public class Encriptacion
    {
        public static string EncriptarSHA512(string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                    var arreglo = Encoding.UTF8.GetBytes(password);
                    var hash = sha512.ComputeHash(arreglo);
                    return Convert.ToHexString(hash).ToLower();
            }
        }
    }
}
