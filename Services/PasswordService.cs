using Microsoft.AspNetCore.Identity;
using UserRegistration_Backend.Models;

namespace UserRegistration_Backend.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        // Hash the password
        public string HashPassword(User user, string password)
        {
            // The PasswordHasher automatically generates a salt and hashes the password
            return _passwordHasher.HashPassword(user, password);
        }

        // Verify the password
        public PasswordVerificationResult VerifyPassword(User user, string password)
        {
            // Verify the password against the stored hash
            return _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        }
    }
}
