using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Airline_Management_System__AMS_.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        // Navigation to passenger profile (if user is a customer who books flights)
        public Passenger? PassengerProfile { get; set; }

        [Display(Name = "Email Confirmation Code")]
        public string EmailConfirmationCode { get; internal set; }

        [Display(Name = "Last Verification Email Sent")]
        public DateTime? LastVerificationEmailSent { get; set; }

        public int VerificationResendCount { get; set; } = 0;


    }
}
