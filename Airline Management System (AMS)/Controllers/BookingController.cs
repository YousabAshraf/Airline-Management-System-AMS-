// Person 3: Booking Controller
// TODO: Implement booking CRUD operations (Customer) and manifest view (Admin)
using Microsoft.AspNetCore.Mvc;

namespace Airline_Management_System__AMS_.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
