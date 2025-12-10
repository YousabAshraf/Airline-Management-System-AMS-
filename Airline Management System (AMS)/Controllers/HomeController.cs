using Airline_Management_System__AMS_.Data;
using Airline_Management_System__AMS_.Models;
using Airline_Management_System__AMS_.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Airline_Management_System__AMS_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(new FlightSearchViewModel
            {
                Origin = string.Empty,
                Destination = string.Empty
            });
        }
        [HttpPost]
        public IActionResult SearchFlights(FlightSearchViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var flights = _context.Flights
                .Where(f =>
                    f.Origin.ToLower() == model.Origin.ToLower() &&
                    f.Destination.ToLower() == model.Destination.ToLower() &&
                    f.DepartureTime.Date == model.DepartureDate.Date)
                .ToList();

            model.SearchResults = flights; // رجّع النتيجة للصفحة

            return View("Index", model);  // يرجع نفس صفحة Home ومعاها النتائج
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
