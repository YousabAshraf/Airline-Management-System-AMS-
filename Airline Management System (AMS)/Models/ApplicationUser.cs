using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Airline_Management_System__AMS_.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
