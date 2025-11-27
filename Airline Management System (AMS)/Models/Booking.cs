using System.ComponentModel.DataAnnotations;

namespace Airline_Management_System__AMS_.Models
{
    public enum BookingStatus
    {
        Booked,
        CheckedIn,
        Cancelled
    }

    public class Booking
    {
        public int Id { get; set; }

        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; }

        [Display(Name = "Seat Number")]
        public string SeatNumber { get; set; }

        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Ticket Price")]
        public decimal TicketPrice { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Booked;
    }
}
