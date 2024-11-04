using System.ComponentModel.DataAnnotations;

namespace ufoShopBack.Models.UsersValidation
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is Required")]
        [Length(4, 50, ErrorMessage = "Username should be 4 to 50 chars")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Eamil Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password should be at least 8 chars")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
