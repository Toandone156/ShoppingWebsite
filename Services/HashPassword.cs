using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using System.Text;

namespace ShoppingWebsite.Services
{
    public class HashPassword
    {
        private static readonly string SALT = "ThisIsSaltPw";

        public static string GetHashPassword(string password)
        {
            byte[] salt = Encoding.Default.GetBytes(SALT);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
