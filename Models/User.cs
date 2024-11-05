using System.ComponentModel.DataAnnotations;

namespace UserRegistration_Backend.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; } = String.Empty;
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; } = String.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = String.Empty;
    }
}
