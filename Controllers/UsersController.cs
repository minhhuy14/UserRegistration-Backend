using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using UserRegistration_Backend.Data;
using UserRegistration_Backend.Models;
using UserRegistration_Backend.Services;

namespace UserRegistration_Backend.Controllers
{
    [Route("/")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRegistration_BackendContext _context;

        private readonly PasswordService _passwordService;
        private readonly AuthService _authService;
        public UsersController(UserRegistration_BackendContext context, AuthService authService, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
            _authService = authService;

        }

        // GET: api/Users
        [HttpGet("/users")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            if (user.Email == null || user.Password == null)
            {
                return BadRequest();
            }
            if (UserNameExists(user.UserName))
            {
                return Conflict(new { message = "Username already exists", userNameExist = true });
            }
            if (UserEmailExists(user.Email))
            {
                return Conflict(new { message = "Email already exists", emailExist = true });
            }
            try
            {
                Console.WriteLine("User: " + user.Id);
                string hashedPassword = _passwordService.HashPassword(user, user.Password);
                user.Password = hashedPassword;

                _context.Add(user);
                await _context.SaveChangesAsync();

                return Ok(
                    new
                    {
                        message = "Register successfully",
                        uid = user.Id,
                        emai = user.Email
                    }
                );
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, dbEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        //POST: /login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            var validPassword = _passwordService.VerifyPassword(user, request.Password);
            if (validPassword == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Invalid password" });
            }

            var accessToken = _authService.Authenticate(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(1),
                SameSite = SameSiteMode.Strict,
                Secure = true
            };
            Response.Cookies.Append("access_token", accessToken, cookieOptions);
            return Ok(new
            {
                message = "Login successfully!",
                email = user.Email,
                uid = user.Id.ToString()
            });


        }

        //GET: /profile
        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult> Profile()
        {
            var uid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.User.FindAsync(Guid.Parse(uid));
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(new
            {
                message = "Profile",
                email = user.Email,
                uid = user.Id.ToString()
            });
        }


        private bool UserEmailExists(string email)
        {
            try
            {
                return _context.User.Any(e => e.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking if user email exists", ex);
            }
        }

        private bool UserNameExists(string username)
        {
            try
            {
                return _context.User.Any(e => e.UserName == username);
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking if username exists", ex);
            }
        }
    }
}
