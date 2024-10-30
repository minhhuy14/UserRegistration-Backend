using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace UserRegistration_Backend.Services
{
    public class PasswordService
    {
        public string HashedPassword(string password)
        {
            // Generate a 128-bit salt using a sequence of
            // cryptographically strong random bytes.
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashedPassword;

        }
    }
}
