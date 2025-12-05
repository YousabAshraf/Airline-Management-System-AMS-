using Microsoft.AspNetCore.Mvc;

namespace Airline_Management_System__AMS_.Controllers
{
    public class FlightController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
