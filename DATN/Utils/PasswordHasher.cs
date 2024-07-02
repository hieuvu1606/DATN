using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace DATN.Utils
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 32));

            string hashedPasswordWithSalt = $"{Convert.ToBase64String(salt)}:{hashedPassword}";

            return hashedPasswordWithSalt;
        }

        public static bool VerifyPassword(string password, string hashedPasswordWithSalt)
        {
            string[] parts = hashedPasswordWithSalt.Split(':');
            byte[] salt = Convert.FromBase64String(parts[0]);
            string hashedPassword = parts[1];

            string hashedProvidedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 32));

            return hashedPassword == hashedProvidedPassword;
        }
    }
}
