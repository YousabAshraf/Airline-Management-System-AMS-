using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Airline_Management_System__AMS_.Data;
using Microsoft.AspNetCore.Identity;
using Airline_Management_System__AMS_.Models;
using Microsoft.EntityFrameworkCore;

namespace Airline_Management_System__AMS_.Controllers
{
     [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminDashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
           
            int numberOfUsers = 0;
            int numberOfAvailableFlights = 0;
            int numberOfCompletedFlights = 0;
            int totalBookings = 0;
            int totalPassengers = 0;
            decimal totalRevenue = 0;
            int todaysFlights = 0;
            int pendingFeedback = 0;
            List<Booking> recentBookings = new List<Booking>();

            try
            {
                
                numberOfUsers = await _userManager.Users.CountAsync();

                numberOfAvailableFlights = await _context.Flights
                    .Where(f => f.DepartureTime > DateTime.Now && f.AvailableSeats > 0)
                    .CountAsync();

                numberOfCompletedFlights = await _context.Flights
                    .Where(f => f.ArrivalTime < DateTime.Now)
                    .CountAsync();

               
                totalBookings = await _context.Bookings.CountAsync();

                totalPassengers = await _context.Passengers
                    .Where(p => !p.IsArchived)
                    .CountAsync();

               
                totalRevenue = await _context.Bookings
                    .Where(b => b.Status != BookingStatus.Cancelled)
                    .SumAsync(b => b.TicketPrice);

                var today = DateTime.Today;
                todaysFlights = await _context.Flights
                    .Where(f => f.DepartureTime.Date == today)
                    .CountAsync();

                pendingFeedback = await _context.Feedbacks.CountAsync();

                
                recentBookings = await _context.Bookings
                    .Include(b => b.Passenger)
                    .Include(b => b.Flight)
                    .OrderByDescending(b => b.BookingDate)
                    .Take(5)
                    .ToListAsync();
            }
            catch (Exception)
            {
               
            }

          
            ViewBag.NumberOfUsers = numberOfUsers;
            ViewBag.NumberOfAvailableFlights = numberOfAvailableFlights;
            ViewBag.NumberOfCompletedFlights = numberOfCompletedFlights;
            ViewBag.TotalBookings = totalBookings;
            ViewBag.TotalPassengers = totalPassengers;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TodaysFlights = todaysFlights;
            ViewBag.PendingFeedback = pendingFeedback;
            ViewBag.RecentBookings = recentBookings;

            return View();
        }

        public IActionResult FlightManagment()
        {
            return View();
        }

        public IActionResult PassengerManagment()
        {
            return View();
        }

        public IActionResult BookingManagment()
        {
            return View();
        }
    }
}
