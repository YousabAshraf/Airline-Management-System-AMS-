using Airline_Management_System__AMS_.Data;
using Airline_Management_System__AMS_.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Airline_Management_System__AMS_.Controllers
{
    public class FlightController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchFlights(FlightSearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var flights = _context.Flights
                .Where(f =>
                    f.Origin.ToLower().Contains(model.Origin.ToLower()) &&
                    f.Destination.ToLower().Contains(model.Destination.ToLower()) &&
                    f.DepartureTime.Date == model.DepartureDate.Date
                )
                .ToList();

            model.SearchResults = flights;

            return View("Index", model);
        }

    }
}
