using System.ComponentModel.DataAnnotations;

namespace E_CommerceSystem_API.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(
      @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$",
      ErrorMessage = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, and one number.")]

        public string Password { get; set; }

        [Required]
        public string Phone { get; set; }


    }
}
