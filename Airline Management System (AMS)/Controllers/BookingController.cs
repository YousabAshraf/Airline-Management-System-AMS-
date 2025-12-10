using Airline_Management_System__AMS_.Data;
using Airline_Management_System__AMS_.Models;
using Airline_Management_System__AMS_.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
[Authorize]
public class BookingController : Controller
{
    private readonly ApplicationDbContext _context;

    public BookingController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Create(BookingViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var booking = new Booking
        {
            FlightId = model.FlightId,
            PassengerId = model.PassengerId,
            SeatNumber = model.SeatNumber,
            TicketPrice = model.TicketPrice,
            Status = BookingStatus.Booked,
            BookingDate = DateTime.Now
        };

        _context.Bookings.Add(booking);

        // Update available seats
        var flight = _context.Flights.Find(model.FlightId);
        flight.AvailableSeats--;

        _context.SaveChanges();

        return RedirectToAction("Details", "Booking", new { id = booking.Id });
    }

    public IActionResult Create(int flightId)
    {
        var flight = _context.Flights
            .Include(f => f.Seats)
            .Include(f => f.Bookings)
            .FirstOrDefault(f => f.FlightId == flightId);

        if (flight == null)
            return NotFound();

        var bookedSeats = flight.Bookings?.Select(b => b.SeatNumber).ToHashSet() ?? new HashSet<string>();
        var availableSeats = flight.Seats
            .Where(s => !bookedSeats.Contains(s.SeatNumber))
            .Select(s => s.SeatNumber)
            .ToList();

        var passenger = _context.Passengers.FirstOrDefault(p => p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));

        var vm = new BookingViewModel
        {
            FlightId = flight.FlightId,
            PassengerId = passenger.Id,
            FlightNumber = flight.FlightNumber,
            Origin = flight.Origin,
            Destination = flight.Destination,
            DepartureTime = flight.DepartureTime,
            AvailableSeats = availableSeats
        };

        ViewBag.Flight = flight;
        ViewBag.Passenger = passenger;

        return View(vm);
    }
}



