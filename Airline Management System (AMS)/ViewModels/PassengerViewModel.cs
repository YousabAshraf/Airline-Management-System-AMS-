using System.ComponentModel.DataAnnotations;

namespace Airline_Management_System__AMS_.ViewModels
{
    public class PassengerViewModel
    {
        public int PassengerId { get; set; } // For Edit

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; } // New field for Admin to set

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\+?[0-9\s-]{8,20}$", ErrorMessage = "Please enter a valid phone number (digits, spaces, or dashes only).")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }

        [Required]
        [Display(Name = "National ID")]
        public string NationalId { get; set; }

        [Required]
        [Display(Name = "Account Role")]
        public string Role { get; set; }
    }
}
