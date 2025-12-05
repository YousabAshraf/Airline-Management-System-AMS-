// Person 2: Feedback Controller
// TODO: Implement feedback submission (Customer) and viewing (Admin/Customer)
using Microsoft.AspNetCore.Mvc;

namespace Airline_Management_System__AMS_.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
