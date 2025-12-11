using Airline_Management_System__AMS_.Data;
using Airline_Management_System__AMS_.Models;
using Airline_Management_System__AMS_.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
public class BookingController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;


    public BookingController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingViewModel model)
    {
        if (model.SelectedSeatId == null)
        {
            ModelState.AddModelError("", "Please select a seat before confirming your booking!");
            return View(model);
        }

        var seat = await _context.Seats.FirstOrDefaultAsync(s => s.SeatId == model.SelectedSeatId.Value);
        if (seat == null || !seat.IsAvailable)
            return BadRequest("Seat not available");

        var booking = new Booking
        {
            FlightId = model.FlightId,
            PassengerId = model.PassengerId,
            SeatNumber = seat.SeatNumber,
            TicketPrice = seat.SeatPrice,
            Status = BookingStatus.Booked,
            BookingDate = DateTime.Now
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(); // Booking.Id موجود بعد الحفظ

        seat.IsAvailable = false;
        seat.BookingId = booking.Id;
        var flight = await _context.Flights.FindAsync(model.FlightId);
        flight.AvailableSeats--;

        await _context.SaveChangesAsync();

        var passenger = await _context.Passengers
            .Include(p => p.User) 
            .FirstOrDefaultAsync(p => p.Id == model.PassengerId);

        TempData["BookingSuccess"] = "Booking successful!";
        return RedirectToAction("Index", "Home");
    }



    [HttpGet]
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

        var passenger = _context.Passengers
            .FirstOrDefault(p => p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));

        int? selectedSeatId = TempData["SelectedSeatId"] as int?;
        Seat selectedSeat = null;

        if (selectedSeatId.HasValue)
        {
            selectedSeat = _context.Seats.FirstOrDefault(s => s.SeatId == selectedSeatId.Value);
        }

        var vm = new BookingViewModel
        {
            FlightId = flight.FlightId,
            PassengerId = passenger.Id,
            FlightNumber = flight.FlightNumber,
            Origin = flight.Origin,
            Destination = flight.Destination,
            DepartureTime = flight.DepartureTime,
            AvailableSeats = availableSeats,
            TicketPrice = selectedSeat?.SeatPrice ?? 0,
            SeatNumber = selectedSeat?.SeatNumber,
            seat = selectedSeat
        };
        ViewBag.Seat = vm.seat;
        ViewBag.Flight = flight;
        ViewBag.Passenger = passenger;

        return View(vm);
    }

    [HttpGet]
    public IActionResult SelectSeat(int flightId)
    {
        var flight = _context.Flights
            .Include(f => f.Seats)
            .Include(f => f.Bookings)
            .FirstOrDefault(f => f.FlightId == flightId);

        if (flight == null)
            return NotFound();

        var seats = flight.Seats.ToList();

        ViewBag.Flight = flight;
        return View(seats);
    }

    [HttpPost]
    public IActionResult SelectSeat(int flightId, Seat seat)
    {
        TempData["SelectedSeatId"] = seat.SeatId;

        return RedirectToAction("Create", new { flightId = flightId });
    }



}



