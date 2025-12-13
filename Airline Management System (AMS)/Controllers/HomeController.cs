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
            if (string.IsNullOrWhiteSpace(model.Origin) || string.IsNullOrWhiteSpace(model.Destination))
            {
                ModelState.AddModelError("", "Origin and Destination are required.");
                return View("Index", model);
            }

            var origin = model.Origin.Trim().ToLower();
            var destination = model.Destination.Trim().ToLower();

            List<Flight> flights = new List<Flight>();

            var departQuery = _context.Flights.AsQueryable();
            departQuery = departQuery.Where(f =>
                f.Origin.ToLower() == origin &&
                f.Destination.ToLower() == destination
            );

            if (model.DepartureDate.HasValue)
            {
                var dep = model.DepartureDate.Value.Date;
                departQuery = departQuery.Where(f => f.DepartureTime.Date == dep);
            }

            flights.AddRange(departQuery.ToList());

            if (model.ReturnDate.HasValue)
            {
                var returnQuery = _context.Flights.AsQueryable();
                returnQuery = returnQuery.Where(f =>
                    f.Origin.ToLower() == destination &&
                    f.Destination.ToLower() == origin &&
                    f.DepartureTime.Date == model.ReturnDate.Value.Date
                );

                flights.AddRange(returnQuery.ToList());
            }

            model.SearchResults = flights;

            return View("Index", model);
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
