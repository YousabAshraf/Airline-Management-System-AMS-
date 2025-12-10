using System.ComponentModel.DataAnnotations;

namespace Airline_Management_System__AMS_.ViewModels
{
    public class RegisterViewModel
    {

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }


        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please select a role")]
        [Display(Name = "Role")]
        public string Role { get; set; }

      
        [Required]
        [Display(Name = "National ID")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "National ID must be 14 digits")]
        public string NationalId { get; set; }

        [Required]
        [Display(Name = "Passport Number")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Passport Number must be between 5 and 20 characters.")]
        public string PassportNumber { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
