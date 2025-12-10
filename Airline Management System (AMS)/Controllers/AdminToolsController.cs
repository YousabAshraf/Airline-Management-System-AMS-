using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Airline_Management_System__AMS_.Data;
using Airline_Management_System__AMS_.Models;

namespace Airline_Management_System__AMS_.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminToolsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminToolsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AdminTools/MigrateUsers
        public async Task<IActionResult> MigrateUsersToPassengers()
        {
            var users = await _userManager.Users.ToListAsync();
            int created = 0;
            int skipped = 0;

            foreach (var user in users)
            {
                // Check if passenger already exists
                var existingPassenger = await _context.Passengers
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);

                if (existingPassenger != null)
                {
                    skipped++;
                    continue;
                }

                // Get user roles
                var roles = await _userManager.GetRolesAsync(user);

                // Only create passenger profiles for Customer/User roles (not Admin)
                if (roles.Contains("User") || roles.Contains("Customer"))
                {
                    var passenger = new Passenger
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber ?? "Not Provided",
                        PassportNumber = "PENDING", // User must update before booking
                        NationalId = null,
                        IsArchived = false
                    };

                    _context.Passengers.Add(passenger);
                    created++;
                }
                else
                {
                    skipped++;
                }
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Migration complete! Created {created} passenger profiles, skipped {skipped} users.";
            return RedirectToAction("Index", "Passenger");
        }
    }
}
