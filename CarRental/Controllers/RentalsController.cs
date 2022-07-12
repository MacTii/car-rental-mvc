using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarRental.Models;

namespace CarRental.Controllers
{
    public class RentalsController : Controller
    {
        private readonly CarRentalDBContext _context;

        public RentalsController(CarRentalDBContext context)
        {
            _context = context;
        }

        // GET: Rentals
        public async Task<IActionResult> Index()
        {
            var carRentalDBContext = _context.Rentals.Include(r => r.Car).Include(r => r.Driver);
            return View(await carRentalDBContext.ToListAsync());
        }

        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null)
            {
                return NotFound();
            }
            return View(rental);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            ViewData["Make"] = new SelectList(_context.Cars, "Id", "Make");
            ViewData["Model"] = new SelectList(_context.Cars, "Id", "Model");
            ViewData["RegistrationNumber"] = new SelectList(_context.Cars, "Id", "RegistrationNumber");
            ViewData["Name"] = new SelectList(_context.Drivers, "Id", "Name");
            ViewData["Surname"] = new SelectList(_context.Drivers, "Id", "Surname");
            ViewData["PhoneNumber"] = new SelectList(_context.Drivers, "Id", "PhoneNumber");
            /*ViewData["CarId"] = new SelectList(_context.Cars, "Id", "CarId");
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "DriverId");*/
            return View();
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarId,DriverId,RentDate,ReturnDate,Comments")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Make"] = new SelectList(_context.Cars, "Id", "Make", rental.CarId);
            ViewData["Model"] = new SelectList(_context.Cars, "Id", "Model", rental.CarId);
            ViewData["RegistrationNumber"] = new SelectList(_context.Cars, "Id", "RegistrationNumber", rental.CarId);
            ViewData["Name"] = new SelectList(_context.Drivers, "Id", "Name", rental.DriverId);
            ViewData["Surname"] = new SelectList(_context.Drivers, "Id", "Surname", rental.DriverId);
            ViewData["PhoneNumber"] = new SelectList(_context.Drivers, "Id", "PhoneNumber", rental.DriverId);
            /*ViewData["CarId"] = new SelectList(_context.Drivers, "Id", "CarId", rental.DriverId);
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "DriverId", rental.DriverId);*/
            return View(rental);
        }

        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            ViewData["Make"] = new SelectList(_context.Cars, "Id", "Make", rental.CarId);
            ViewData["Model"] = new SelectList(_context.Cars, "Id", "Model", rental.CarId);
            ViewData["RegistrationNumber"] = new SelectList(_context.Cars, "Id", "RegistrationNumber", rental.CarId);
            ViewData["Name"] = new SelectList(_context.Drivers, "Id", "Name", rental.DriverId);
            ViewData["Surname"] = new SelectList(_context.Drivers, "Id", "Surname", rental.DriverId);
            ViewData["PhoneNumber"] = new SelectList(_context.Drivers, "Id", "PhoneNumber", rental.DriverId);
            return View(rental);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Make,Model,RegistrationNumber,Name,Surname,PhoneNumber,RentDate,ReturnDate,Comments")] Rental rental)
        {
            if (id != rental.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.Id))
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
            ViewData["Make"] = new SelectList(_context.Cars, "Id", "Make", rental.CarId);
            ViewData["Model"] = new SelectList(_context.Cars, "Id", "Model", rental.CarId);
            ViewData["RegistrationNumber"] = new SelectList(_context.Cars, "Id", "RegistrationNumber", rental.CarId);
            ViewData["Name"] = new SelectList(_context.Drivers, "Id", "Name", rental.DriverId);
            ViewData["Surname"] = new SelectList(_context.Drivers, "Id", "Surname", rental.DriverId);
            ViewData["PhoneNumber"] = new SelectList(_context.Drivers, "Id", "PhoneNumber", rental.DriverId);
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(e => e.Id == id);
        }

        /*[HttpGet]
        public ActionResult GetRentals()
        {
            var rentals = _context.Drivers.ToList();
            return Json(rentals);
        }*/
    }
}
