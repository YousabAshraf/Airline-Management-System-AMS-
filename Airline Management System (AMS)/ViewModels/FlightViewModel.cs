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


    }
}
