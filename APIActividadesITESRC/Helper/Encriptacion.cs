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
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha512.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                foreach (byte item in hash)
                {
                    builder.Append(item.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
