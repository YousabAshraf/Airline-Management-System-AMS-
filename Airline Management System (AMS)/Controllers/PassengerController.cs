using Airline_Management_System__AMS_.Data;
using Airline_Management_System__AMS_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Airline_Management_System__AMS_.ViewModels; 
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

       
        public async Task<IActionResult> Index()
        {
            var passengers = await _context.Passengers
                .Include(p => p.User)
                .ToListAsync();
            return View(passengers);
        }

        
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

      
        public IActionResult Create()
        {
            return View();
        }


        
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
                        Role = model.Role 
                    };

                    
                    var result = await _userManager.CreateAsync(newUser, model.Password);

                    if (result.Succeeded)
                    {
                        
                        if (!await _roleManager.RoleExistsAsync(model.Role))
                        {
                            await _roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole(model.Role));
                        }

                        
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

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger != null)
            {
               
                passenger.IsArchived = true;
                _context.Update(passenger);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDelete(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var passenger = await _context.Passengers
                    .Include(p => p.Bookings)
                    // .ThenInclude(b => b.Seat) // Removed: Booking does not have Seat navigation property
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (passenger == null)
                {
                    return NotFound();
                }

                
                if (passenger.Bookings != null && passenger.Bookings.Any())
                {
                    foreach (var booking in passenger.Bookings)
                    {
                        // Some bookings might reference a seat
                      
                        var seat = await _context.Seats.FirstOrDefaultAsync(s => s.BookingId == booking.Id);
                        if (seat != null)
                        {
                            seat.BookingId = null;
                            seat.IsAvailable = true;
                            _context.Seats.Update(seat);
                        }
                    }
                    // Remove bookings (this updates Revenue stats indirectly as they are calculated from Bookings table)
                    _context.Bookings.RemoveRange(passenger.Bookings);
                }

                
                if (!string.IsNullOrEmpty(passenger.UserId))
                {
                    var user = await _userManager.FindByIdAsync(passenger.UserId);
                    if (user != null)
                    {
                        var feedbacks = await _context.Feedbacks.Where(f => f.UserId == passenger.UserId).ToListAsync();
                        if (feedbacks.Any())
                        {
                            _context.Feedbacks.RemoveRange(feedbacks);
                        }

                        
                        var result = await _userManager.DeleteAsync(user);
                        if (!result.Succeeded)
                        {
                            // If delete fails (e.g., other constraints), throw to rollback transaction
                            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                            throw new Exception($"Failed to delete user account: {errors}");
                        }
                    }
                }

                
                _context.Passengers.Remove(passenger);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Passenger, bookings, and user account permanently deleted.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "Error deleting passenger: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PassengerExists(int id)
        {
            return _context.Passengers.Any(e => e.Id == id);
        }
    }
}
