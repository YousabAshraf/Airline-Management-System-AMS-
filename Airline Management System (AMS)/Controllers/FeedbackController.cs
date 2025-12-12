using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Airline_Management_System__AMS_.Data;
using Airline_Management_System__AMS_.Models;

namespace Airline_Management_System__AMS_.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FeedbackController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Feedback/Index (Admin Dashboard for Feedback)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var feedback = await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Flight)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
            return View(feedback);
        }

        // GET: Feedback/Submit
        public async Task<IActionResult> Submit(int? flightId)
        {
            var user = await _userManager.GetUserAsync(User);

            // Populate flights dropdown for selection if not pre-selected
            ViewData["FlightId"] = new SelectList(_context.Flights.Where(f => f.DepartureTime < DateTime.Now), "Id", "FlightInfo");

            if (flightId.HasValue)
            {
                var feedback = new Feedback { FlightId = flightId.Value };
                return View(feedback);
            }

            return View();
        }

        // POST: Feedback/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit([Bind("FlightId,Rating,Comment")] Feedback feedback)
        {
            var user = await _userManager.GetUserAsync(User);
            feedback.UserId = user.Id;
            feedback.CreatedAt = DateTime.Now;

            // Remove User and UserId from validation since we set them manually
            ModelState.Remove("User");
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thank you for your feedback!";
                return RedirectToAction(nameof(MyFeedback));
            }

            // Reload dropdown if validation fails
            ViewData["FlightId"] = new SelectList(_context.Flights.Where(f => f.DepartureTime < DateTime.Now), "Id", "FlightInfo", feedback.FlightId);
            return View(feedback);
        }

        // GET: Feedback/MyFeedback
        public async Task<IActionResult> MyFeedback()
        {
            var user = await _userManager.GetUserAsync(User);
            var myFeedback = await _context.Feedbacks
                .Include(f => f.Flight)
                .Where(f => f.UserId == user.Id)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            return View(myFeedback);
        }

        // POST: Feedback/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
