using System.ComponentModel.DataAnnotations;

namespace Airline_Management_System__AMS_.ViewModels
{
    public class FlightViewModel
    {
        public int FlightId { get; set; }

        [Required]
        [Display(Name = "Flight Number")]
        public string FlightNumber { get; set; }

        [Required]
        [Display(Name = "Origin")]
        public string Origin { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public string Destination { get; set; }

        [Required]
        [Display(Name = "Departure Time")]
        [DataType(DataType.DateTime)]
        public DateTime DepartureTime { get; set; }

        [Required]
        [Display(Name = "Arrival Time")]
        [DataType(DataType.DateTime)]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [Display(Name = "Aircraft Type")]
        public string AircraftType { get; set; }

        [Required]
        [Display(Name = "Total Seats")]
        public int TotalSeats { get; set; }

        [Display(Name = "Available Seats")]
        public int AvailableSeats { get; set; }

        [Display(Name = "Booked Seats")]
        public int BookedSeats => TotalSeats - AvailableSeats;

        // Economy Class Properties
        [Required]
        [Display(Name = "Economy Seats")]
        public int EconomySeats { get; set; }

        [Required]
        [Display(Name = "Economy Price")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public int EconomyPrice { get; set; } = 100;

        // Business Class Properties
        [Required]
        [Display(Name = "Business Seats")]
        public int BusinessSeats { get; set; }

        [Required]
        [Display(Name = "Business Price")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public int BusinessPrice { get; set; } = 250;

        // First Class Properties
        [Required]
        [Display(Name = "First Class Seats")]
        public int FirstClassSeats { get; set; }

        [Required]
        [Display(Name = "First Class Price")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public int FirstClassPrice { get; set; } = 500;
    }
}
