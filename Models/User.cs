using System.ComponentModel.DataAnnotations;

namespace UserRegistration_Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
