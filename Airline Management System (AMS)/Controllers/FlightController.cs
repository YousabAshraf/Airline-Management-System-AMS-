using Airline_Management_System__AMS_.Data;
using Airline_Management_System__AMS_.Models;
using Airline_Management_System__AMS_.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Airline_Management_System__AMS_.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FlightController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var flights = await _context.Flights
                .Include(f => f.Seats)
                .Include(f => f.Bookings)
                .ToListAsync();
            return View(flights);
        }

        // GET: Flight/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.Seats)
                .Include(f => f.Bookings)
                    .ThenInclude(b => b.Passenger)
                .FirstOrDefaultAsync(f => f.FlightId == id);

            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flight/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flight/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightNumber,Origin,Destination,DepartureTime,ArrivalTime,AircraftType,EconomySeats,EconomyPrice,BusinessSeats,BusinessPrice,FirstClassSeats,FirstClassPrice")] FlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Validate that at least one seat class has seats
                if (model.EconomySeats == 0 && model.BusinessSeats == 0 && model.FirstClassSeats == 0)
                {
                    ModelState.AddModelError("", "At least one seat class must have seats.");
                    return View(model);
                }

                // Validate seat numbers are positive
                if (model.EconomySeats < 0 || model.BusinessSeats < 0 || model.FirstClassSeats < 0)
                {
                    ModelState.AddModelError("", "Seat numbers cannot be negative.");
                    return View(model);
                }

                // Calculate total seats
                int totalSeats = model.EconomySeats + model.BusinessSeats + model.FirstClassSeats;

                var flight = new Flight
                {
                    FlightNumber = model.FlightNumber,
                    Origin = model.Origin,
                    Destination = model.Destination,
                    DepartureTime = model.DepartureTime,
                    ArrivalTime = model.ArrivalTime,
                    AircraftType = model.AircraftType,
                    AvailableSeats = totalSeats
                };

                _context.Add(flight);
                await _context.SaveChangesAsync();

                // Create seats for each class
                var seats = new List<Seat>();
                int seatCounter = 1;

                // Create Economy Class Seats
                for (int i = 1; i <= model.EconomySeats; i++)
                {
                    seats.Add(new Seat
                    {
                        FlightId = flight.FlightId,
                        SeatNumber = $"E{seatCounter:D3}",
                        Class = "Economy",
                        SeatPrice = model.EconomyPrice,
                        IsAvailable = true
                    });
                    seatCounter++;
                }

                // Create Business Class Seats
                seatCounter = 1;
                for (int i = 1; i <= model.BusinessSeats; i++)
                {
                    seats.Add(new Seat
                    {
                        FlightId = flight.FlightId,
                        SeatNumber = $"B{seatCounter:D3}",
                        Class = "Business",
                        SeatPrice = model.BusinessPrice,
                        IsAvailable = true
                    });
                    seatCounter++;
                }

                // Create First Class Seats
                seatCounter = 1;
                for (int i = 1; i <= model.FirstClassSeats; i++)
                {
                    seats.Add(new Seat
                    {
                        FlightId = flight.FlightId,
                        SeatNumber = $"F{seatCounter:D3}",
                        Class = "First Class",
                        SeatPrice = model.FirstClassPrice,
                        IsAvailable = true
                    });
                    seatCounter++;
                }

                _context.Seats.AddRange(seats);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Flight created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Flight/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.Seats)
                .FirstOrDefaultAsync(f => f.FlightId == id);

            if (flight == null)
            {
                return NotFound();
            }

            var model = new FlightViewModel
            {
                FlightId = flight.FlightId,
                FlightNumber = flight.FlightNumber,
                Origin = flight.Origin,
                Destination = flight.Destination,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                AircraftType = flight.AircraftType,
                TotalSeats = flight.Seats?.Count ?? 0,
                AvailableSeats = flight.AvailableSeats
            };

            return View(model);
        }

        // POST: Flight/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightId,FlightNumber,Origin,Destination,DepartureTime,ArrivalTime,AircraftType")] FlightViewModel model)
        {
            if (id != model.FlightId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var flight = await _context.Flights.FindAsync(id);
                    if (flight == null)
                    {
                        return NotFound();
                    }

                    flight.FlightNumber = model.FlightNumber;
                    flight.Origin = model.Origin;
                    flight.Destination = model.Destination;
                    flight.DepartureTime = model.DepartureTime;
                    flight.ArrivalTime = model.ArrivalTime;
                    flight.AircraftType = model.AircraftType;

                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(model.FlightId))
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
            return View(model);
        }

        // GET: Flight/Delete/5 (Delete Confirmation)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.Bookings)
                .FirstOrDefaultAsync(f => f.FlightId == id);

            if (flight == null)
            {
                return NotFound();
            }

            // Check if flight has bookings
            if (flight.Bookings != null && flight.Bookings.Any())
            {
                TempData["Error"] = "Cannot delete a flight with existing bookings. Please cancel all bookings first.";
                return RedirectToAction(nameof(Index));
            }

            return View(flight);
        }

        // POST: Flight/Delete/5 (Performs Hard Delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flights
                .Include(f => f.Bookings)
                .Include(f => f.Seats)
                .FirstOrDefaultAsync(f => f.FlightId == id);

            if (flight == null)
            {
                return NotFound();
            }

            // Check if flight has bookings
            if (flight.Bookings != null && flight.Bookings.Any())
            {
                TempData["Error"] = "Cannot delete a flight with existing bookings. Please cancel all bookings first.";
                return RedirectToAction(nameof(Index));
            }

            // Remove associated seats first
            if (flight.Seats != null && flight.Seats.Any())
            {
                _context.Seats.RemoveRange(flight.Seats);
            }

            // Hard delete the flight
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Flight deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.FlightId == id);
        }
    }
}
