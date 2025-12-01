using System.ComponentModel.DataAnnotations;

namespace Airline_Management_System__AMS_.Models
{
    public class Passenger
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string dateOfBirth { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Passport Number")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Passport Number must be between 5 and 20 characters.")]
        public string PassportNumber { get; set; }

        [Display(Name = "National ID")]
        [StringLength(20)]
        public string? NationalId { get; set; }

        [Display(Name = "Archived")]
        public bool IsArchived { get; set; } = false;

        public ICollection<Booking> Bookings { get; set; }

    }
}
