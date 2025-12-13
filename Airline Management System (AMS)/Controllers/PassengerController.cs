using Airline_Management_System__AMS_.Data;
using Airline_Management_System__AMS_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using Airline_Management_System__AMS_.ViewModels; // Added for ViewModel support
using Airline_Management_System__AMS_.Controllers;
namespace Airline_Management_System__AMS_.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PassengerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> _roleManager;

        public PassengerController(ApplicationDbContext context,
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Passenger
        public async Task<IActionResult> Index()
        {
            var passengers = await _context.Passengers.ToListAsync();
            return View(passengers);
        }

        // GET: Passenger/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers
                .Include(p => p.Bookings)
                .ThenInclude(b => b.Flight)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // GET: Passenger/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Passenger/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PassengerViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 0. Uniqueness Validation
                if (await _context.Passengers.AnyAsync(p => p.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "A passenger with this Email Address already exists.");
                }
                if (await _context.Passengers.AnyAsync(p => p.PassportNumber == model.PassportNumber))
                {
                    ModelState.AddModelError("PassportNumber", "A passenger with this Passport Number already exists.");
                }
                if (!string.IsNullOrEmpty(model.NationalId) && await _context.Passengers.AnyAsync(p => p.NationalId == model.NationalId))
                {
                    ModelState.AddModelError("NationalId", "A passenger with this National ID already exists.");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // 1. Create/Check User Account
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                string userId = null;

                if (existingUser == null)
                {
                    var newUser = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        EmailConfirmed = true,
                        PhoneNumber = model.PhoneNumber,
                        PhoneNumberConfirmed = true,
                        NationalId = model.NationalId,
                        PassportNumber = model.PassportNumber,
                        Role = model.Role // Map the selected Role
                    };

                    // Use the password provided by the Admin
                    var result = await _userManager.CreateAsync(newUser, model.Password);

                    if (result.Succeeded)
                    {
                        // Ensure the Role exists in the database
                        if (!await _roleManager.RoleExistsAsync(model.Role))
                        {
                            await _roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole(model.Role));
                        }

                        // Add to the selected Role (Admin or Customer)
                        await _userManager.AddToRoleAsync(newUser, model.Role);

                        userId = newUser.Id;
                        TempData["SuccessMessage"] = "Passenger and User Account created successfully.";
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, $"User Creation Failed: {error.Description}");
                        }
                        return View(model);
                    }
                }
                else
                {
                    userId = existingUser.Id;
                }

                // 2. Create Passenger
                var passenger = new Passenger
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PassportNumber = model.PassportNumber,
                    NationalId = model.NationalId,
                    UserId = userId
                };

                _context.Add(passenger);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Passenger/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger == null)
            {
                return NotFound();
            }
            return View(passenger);
        }

        // POST: Passenger/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber,PassportNumber,NationalId,IsArchived")] Passenger passenger)
        {
            if (id != passenger.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passenger);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassengerExists(passenger.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(passenger);
        }

        // GET: Passenger/Delete/5 (Archive Confirmation)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // POST: Passenger/Delete/5 (Performs Archive/Soft Delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger != null)
            {
                // Soft delete (Archive)
                passenger.IsArchived = true;
                _context.Update(passenger);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Passenger/Restore/5 (Un-archive)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger != null)
            {
                passenger.IsArchived = false;
                _context.Update(passenger);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PassengerExists(int id)
        {
            return _context.Passengers.Any(e => e.Id == id);
        }
    }
}
