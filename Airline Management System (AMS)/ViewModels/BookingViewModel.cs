using System.ComponentModel.DataAnnotations;

public class BookingViewModel
{
    public int FlightId { get; set; }
    public int PassengerId { get; set; }

    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureTime { get; set; }
    public decimal TicketPrice { get; set; }

    public List<string> AvailableSeats { get; set; }

    [Required]
    [Display(Name = "Seat Number")]
    public string SeatNumber { get; set; }
}
