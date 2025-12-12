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

[Authorize(Roles = "Admin,User")]
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
        /*await _emailSender.SendEmailAsync(
            passenger.Email,
            "Booking Confirmation - Airline Management System",
            $@"
<div style='font-family: Arial, sans-serif; background-color:#f5f7fa; padding:30px;'>
    <div style='max-width:600px; margin:auto; background:white; padding:25px; border-radius:12px; box-shadow:0 2px 10px rgba(0,0,0,0.08);'>
        <!-- Banner -->
        <div style='text-align:center; margin-bottom:20px;'>
            <img src='https://github.com/Steven-Amin02/Airline-Management-System-AMS-/raw/master/Airline%20Management%20System-AMS-/wwwroot/images/logo_in_email/readme_banner.png'
                 alt='Banner'
                 style='width:100%; border-radius:10px;' />
        </div>

        <!-- Title -->
        <h2 style='text-align:center; color:#333; margin-bottom:10px;'>
            Booking Confirmed!
        </h2>

        <p style='color:#555; font-size:15px; text-align:center;'>
            Hello <strong>{passenger.FullName}</strong>,<br/>
            Your booking has been successfully confirmed. Here are your ticket details:
        </p>

        <!-- Booking Details Box -->
        <div style='margin:30px auto; text-align:center;'>
            <div style='display:inline-block; padding:15px 25px; border-radius:10px;
                        background:#004aad; color:white; font-size:16px; 
                        letter-spacing:1px; font-weight:bold; text-align:left;'>
                <p><strong>Flight:</strong> {flight.FlightNumber}</p>
                <p><strong>Seat Number:</strong> {model.seat.SeatNumber}</p>
                <p><strong>Class:</strong> {model.seat.Class}</p>
                <p><strong>Departure:</strong> {flight.DepartureTime:f}</p>
            </div>
        </div>

        <p style='color:#555; font-size:14px; text-align:center;'>
            Please be at the airport at least 2 hours before departure.
        </p>

        <hr style='margin:30px 0; border:none; border-top:1px solid #ddd;'>

        <p style='text-align:center; font-size:13px; color:#888;'>
            If you didn't make this booking, please contact our support immediately.
        </p>
    </div>
</div>
");
        */
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


    public async Task<IActionResult> MyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var passenger = await _context.Passengers
            .Include(p => p.Bookings)
                .ThenInclude(b => b.Flight)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (passenger == null)
            return NotFound("Passenger not found.");

        var bookings = passenger.Bookings?.OrderByDescending(b => b.BookingDate).ToList() ?? new List<Booking>();

        return View(bookings);
    }


    public async Task<IActionResult> Details(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var booking = await _context.Bookings
            .Include(b => b.Flight)
            .Include(b => b.Passenger)
            .FirstOrDefaultAsync(b => b.Id == id && b.Passenger.UserId == userId);

        if (!CanAccessBooking(booking)) return Forbid(); 

        if (booking == null)
            return NotFound();

        var seat = await _context.Seats.FirstOrDefaultAsync(s => s.BookingId == booking.Id);

        ViewBag.Seat = seat;
        return View(booking);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


        var booking = await _context.Bookings
            .Include(b => b.Flight)
            .Include(b => b.Passenger)
            .FirstOrDefaultAsync(b => b.Id == id && b.Passenger.UserId == userId);
        var seat = await _context.Seats.FirstOrDefaultAsync(s => s.BookingId == booking.Id);

        

        if (booking == null)
            return NotFound();

        if (!CanAccessBooking(booking)) return Forbid(); 

        // تحقق أن الحالة Booked فقط يمكن إلغاؤها
        if (booking.Status != BookingStatus.Booked)
            return BadRequest("This booking cannot be cancelled.");

        var passenger = await _context.Passengers
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == booking.PassengerId);
       /* await _emailSender.SendEmailAsync(
    passenger.Email,
    "Booking Cancelled - Airline Management System",
    $@"
<div style='font-family: Arial, sans-serif; background-color:#f5f7fa; padding:30px;'>
    <div style='max-width:600px; margin:auto; background:white; padding:25px; border-radius:12px; box-shadow:0 2px 10px rgba(0,0,0,0.08);'>
        <!-- Banner -->
        <div style='text-align:center; margin-bottom:20px;'>
            <img src='https://github.com/Steven-Amin02/Airline-Management-System-AMS-/raw/master/Airline%20Management%20System-AMS-/wwwroot/images/logo_in_email/readme_banner.png'
                 alt='Banner'
                 style='width:100%; border-radius:10px;' />
        </div>

        <!-- Title -->
        <h2 style='text-align:center; color:#dc3545; margin-bottom:10px;'>
            Booking Cancelled
        </h2>

        <p style='color:#555; font-size:15px; text-align:center;'>
            Hello <strong>{passenger.FullName}</strong>,<br/>
            We regret to inform you that your booking has been cancelled. Here are the details of the cancelled ticket:
        </p>

        <!-- Booking Details Box -->
        <div style='margin:30px auto; text-align:center;'>
            <div style='display:inline-block; padding:15px 25px; border-radius:10px;
                        background:#dc3545; color:white; font-size:16px; 
                        letter-spacing:1px; font-weight:bold; text-align:left;'>
                <p><strong>Flight:</strong> {flight.FlightNumber}</p>
                <p><strong>Seat Number:</strong> {model.seat.SeatNumber}</p>
                <p><strong>Class:</strong> {model.seat.Class}</p>
                <p><strong>Original Departure:</strong> {flight.DepartureTime:f}</p>
            </div>
        </div>

        <p style='color:#555; font-size:14px; text-align:center;'>
            If you believe this is a mistake or need further assistance, please contact our support team.
        </p>

        <hr style='margin:30px 0; border:none; border-top:1px solid #ddd;'>

        <p style='text-align:center; font-size:13px; color:#888;'>
            Thank you for using Airline Management System.
        </p>
    </div>
</div>
");*/

        booking.Status = BookingStatus.Cancelled;

        seat.IsAvailable = true;

        var flight = await _context.Flights.FindAsync(booking.FlightId);
        flight.AvailableSeats++;

        if (seat != null)
        {
            seat.BookingId = null;
        }
        await _context.SaveChangesAsync();

        


        return RedirectToAction("MyBookings");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int bookingId)
    {

        var booking = await _context.Bookings
            .Include(b => b.Flight)
            .Include(b => b.Passenger)
            .FirstOrDefaultAsync(b => b.Id == bookingId);


        if (booking == null) return NotFound();

        if (!CanAccessBooking(booking)) return Forbid();

        var bookedSeats = _context.Bookings
            .Where(b => b.FlightId == booking.FlightId && b.Id != booking.Id)
            .Select(b => b.SeatNumber)
            .ToHashSet();

        var seats = await _context.Seats
            .Where(s => s.FlightId == booking.FlightId)
            .ToListAsync();

        var vm = new BookingEditViewModel
        {
            BookingId = booking.Id,
            FlightId = booking.FlightId.Value,
            PassengerId = booking.PassengerId,
            CurrentSeat = booking.SeatNumber,
            TicketPrice = booking.TicketPrice,
            Seats = seats,
            BookedSeats = bookedSeats,
            Class= seats.FirstOrDefault(s => s.SeatNumber == booking.SeatNumber)?.Class
        };

        ViewBag.Flight = booking.Flight;
        ViewBag.Passenger = booking.Passenger;

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int bookingId, int selectedSeatId)
    {
        var booking = await _context.Bookings
            .Include(b => b.Flight)
            .Include(b => b.Passenger)
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking == null) return NotFound();

        if (!CanAccessBooking(booking)) return Forbid();

        var newSeat = await _context.Seats.FirstOrDefaultAsync(s => s.SeatId == selectedSeatId);

        if (newSeat == null || !newSeat.IsAvailable)
            return BadRequest("Selected seat is not available.");

        var oldSeat = await _context.Seats
            .FirstOrDefaultAsync(s => s.SeatNumber == booking.SeatNumber && s.FlightId == booking.FlightId);

        if (oldSeat != null)
        {
            oldSeat.IsAvailable = true;
            oldSeat.BookingId = null;
        }

        newSeat.IsAvailable = false;
        newSeat.BookingId = booking.Id;

        booking.SeatNumber = newSeat.SeatNumber;
        booking.TicketPrice = newSeat.SeatPrice;

        await _context.SaveChangesAsync();

        // SEND UPDATED BOOKING EMAIL
       /* await _emailSender.SendEmailAsync(
            booking.Passenger.Email,
            "Booking Updated Successfully - Airline Management System",
            $@"
<div style='font-family: Arial, sans-serif; background-color:#f5f7fa; padding:30px;'>
    <div style='max-width:600px; margin:auto; background:white; padding:25px; border-radius:12px; box-shadow:0 2px 10px rgba(0,0,0,0.08);'>

        <div style='text-align:center; margin-bottom:20px;'>
            <img src='https://github.com/Steven-Amin02/Airline-Management-System-AMS-/raw/master/Airline%20Management%20System-AMS-/wwwroot/images/logo_in_email/readme_banner.png'
                 alt='Banner'
                 style='width:100%; border-radius:10px;' />
        </div>

        <h2 style='text-align:center; color:#333; margin-bottom:10px;'>
            Booking Updated Successfully!
        </h2>

        <p style='color:#555; font-size:15px; text-align:center;'>
            Hello <strong>{booking.Passenger.FullName}</strong>,<br/>
            Your booking has been successfully updated. Here are your new ticket details:
        </p>

        <div style='margin:30px auto; text-align:center;'>
            <div style='display:inline-block; padding:15px 25px; border-radius:10px;
                        background:#0a6efd; color:white; font-size:16px; 
                        letter-spacing:1px; font-weight:bold; text-align:left;'>
                <p><strong>Flight:</strong> {booking.Flight.FlightNumber}</p>
                <p><strong>New Seat Number:</strong> {booking.SeatNumber}</p>
                <p><strong>Class:</strong> {newSeat.Class}</p>
                <p><strong>New Price:</strong> {newSeat.SeatPrice} EGP</p>
                <p><strong>Departure:</strong> {booking.Flight.DepartureTime:f}</p>
                <p><strong>Arrival:</strong> {booking.Flight.ArrivalTime:f}</p>
            </div>
        </div>

        <p style='color:#555; font-size:14px; text-align:center;'>
            Please make sure to arrive at the airport at least 2 hours before departure time.
        </p>

        <hr style='margin:30px 0; border:none; border-top:1px solid #ddd;'>

        <p style='text-align:center; font-size:13px; color:#888;'>
            If you did not request this update, please contact our support team immediately.
        </p>
    </div>
</div>
");*/

        TempData["BookingSuccess"] = "Booking updated successfully!";
        return RedirectToAction("MyBookings");
    }



    private bool CanAccessBooking(Booking booking)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return User.IsInRole("Admin") || booking.Passenger.UserId == userId;
    }

}



