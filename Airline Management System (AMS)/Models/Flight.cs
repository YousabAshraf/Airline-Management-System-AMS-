using System.ComponentModel.DataAnnotations;

namespace Airline_Management_System__AMS_.Models
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        [Required]
        [Display(Name = "Flight Number")]
        public string FlightNumber { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        [Display(Name = "Departure Time")]
        public DateTime DepartureTime { get; set; }

        [Required]
        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [Display(Name = "Aircraft Type")]
        public string AircraftType { get; set; }

        [Required]
        [Display(Name = "Available Seats")]
        public int AvailableSeats { get; set; }

        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}
