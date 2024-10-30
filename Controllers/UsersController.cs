using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using UserRegistration_Backend.Data;
using UserRegistration_Backend.Models;
using UserRegistration_Backend.Services;

namespace UserRegistration_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRegistration_BackendContext _context;

        public UsersController(UserRegistration_BackendContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
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
        public async Task<ActionResult<User>> PostUser(User user)
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
                string hashedPassword = new PasswordService().HashedPassword(user.Password);
                user.Password = hashedPassword;

                _context.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.Id }, user);
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
