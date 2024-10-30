using System.ComponentModel.DataAnnotations;

namespace UserRegistration_Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
