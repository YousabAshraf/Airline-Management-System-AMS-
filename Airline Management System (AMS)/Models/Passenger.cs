using System.ComponentModel.DataAnnotations;

namespace Airline_Management_System__AMS_.Models
{
    public class Passenger
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }

        [Display(Name = "National ID")]
        public string? NationalId { get; set; }
    }
}
