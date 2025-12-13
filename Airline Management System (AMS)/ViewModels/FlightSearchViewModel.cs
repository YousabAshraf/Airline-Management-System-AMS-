using Airline_Management_System__AMS_.Models;
using System.ComponentModel.DataAnnotations;

namespace Airline_Management_System__AMS_.ViewModels
{
    public class FlightSearchViewModel
    {
        [Required(ErrorMessage = "Origin is required")]
        [Display(Name = "From")]
        public required string Origin { get; set; }

        [Required(ErrorMessage = "Destination is required")]
        [Display(Name = "To")]
        public required string Destination { get; set; }

        [Display(Name = "Departure Date")]
        [DataType(DataType.Date)]
        public DateTime? DepartureDate { get; set; }

        [Display(Name = "Return Date")]
        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        [Required]
        [Range(1, 9, ErrorMessage = "Passenger count must be between 1 and 9")]
        [Display(Name = "Passengers")]
        public int PassengerCount { get; set; } = 1;

        public List<Flight>? SearchResults { get; set; }
    }
}
