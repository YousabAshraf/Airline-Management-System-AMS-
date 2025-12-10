using Airline_Management_System__AMS_.Data;
using Airline_Management_System__AMS_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Airline_Management_System__AMS_.Controllers
{
    public class PassengerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PassengerController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> IndexAsync()
        {
            var passengers = await _context.Passengers.ToListAsync();
            return View(passengers);
        }

        // GET: Passenger/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Passenger/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Passenger passanger)
        {
            if (ModelState.IsValid)
            {
                _context.Passengers.Add(passanger);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(passanger);
        }

        // GET: Passenger/Edit/5
        public IActionResult Edit()
        {
            return View();
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

            return View();
        }

        // POST: Passenger/Edit/5
        public async Task<IActionResult> Edit(int id, Passenger passenger)
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

        private bool PassengerExists(int id)
        {
            return _context.Passengers.Any(e => e.Id == id);
        }

        // GET: Passenger/Delete/5
        public IActionResult Delete()
        {
            return View();
        }

        // GET: Passenger/Delete/5
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

        // POST: Passenger/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passenger = await _context.Passengers.FindAsync(id);
            _context.Passengers.Remove(passenger);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
