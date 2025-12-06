using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Airline_Management_System__AMS_.Data;
using Microsoft.AspNetCore.Identity;
using Airline_Management_System__AMS_.Models;
using Microsoft.EntityFrameworkCore;

namespace Airline_Management_System__AMS_.Controllers
{
   // [Authorize(Roles = "Admin")]
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
            // Initialize default values in case database connection fails
            int numberOfUsers = 0;
            int numberOfAvailableFlights = 0;
            int numberOfCompletedFlights = 0;

            try
            {
                // Get statistics
                numberOfUsers = await _userManager.Users.CountAsync();
                numberOfAvailableFlights = await _context.Flights
                    .Where(f => f.DepartureTime > DateTime.Now && f.AvailableSeats > 0)
                    .CountAsync();
                numberOfCompletedFlights = await _context.Flights
                    .Where(f => f.ArrivalTime < DateTime.Now)
                    .CountAsync();
            }
            catch (Exception)
            {
                // If database connection fails, use default values (0)
                // This allows the page to load even if SQL Server is not running
            }

            ViewBag.NumberOfUsers = numberOfUsers;
            ViewBag.NumberOfAvailableFlights = numberOfAvailableFlights;
            ViewBag.NumberOfCompletedFlights = numberOfCompletedFlights;

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
