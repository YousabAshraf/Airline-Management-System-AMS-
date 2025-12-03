using System.Diagnostics;
using Airline_Management_System__AMS_.Models;
using Airline_Management_System__AMS_.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Airline_Management_System__AMS_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            {
                return View("Index", model);
            }

            // TODO: Implement flight search logic when FlightsController is created
            // For now, redirect to home with a message
            TempData["SearchMessage"] = $"Searching for flights from {model.Origin} to {model.Destination}...";
            return RedirectToAction("Index");
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
