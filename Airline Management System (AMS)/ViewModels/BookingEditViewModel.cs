using Airline_Management_System__AMS_.Models;
using System.ComponentModel.DataAnnotations;

namespace Airline_Management_System__AMS_.ViewModels
{
    public class BookingEditViewModel
    {
        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }

        public string CurrentSeat { get; set; }
        public decimal TicketPrice { get; set; }

        public string Class { get; set; }

        public int SelectedSeatId { get; set; }

        public List<Seat> Seats { get; set; }
        public HashSet<string> BookedSeats { get; set; }
    }

}
