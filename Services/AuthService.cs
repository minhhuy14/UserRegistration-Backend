using System.Security.Claims;
using UserRegistration_Backend.Models;

namespace UserRegistration_Backend.Services
{
    public class AuthService
    {
        private readonly TokenHelper _tokenHelper;

        public AuthService(TokenHelper tokenHelper)
        {
            _tokenHelper = tokenHelper;
        }

        public string Authenticate(User user)
        {
            // Generate a token for the authenticated user
            return _tokenHelper.GenerateToken(user.Id.ToString(), user.Email, user.UserName);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            // Validate the provided token
            return _tokenHelper.ValidateToken(token);
        }
    }
}
