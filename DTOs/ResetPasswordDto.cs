using System.ComponentModel.DataAnnotations;

namespace NOLA_API.DTOs
{
    public class ResetPasswordDto
    {
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$", ErrorMessage = "Password must contain Uppercase char, Lowercase char, Special symbol and be 8-16 symbols long!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
