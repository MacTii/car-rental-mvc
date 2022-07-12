using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarRental.Models;
using CarRental.Repositories;

namespace CarRental.Controllers
{
    public class Rentals1Controller : Controller
    {
        private readonly CarRentalDBContext _context;
        private readonly IGenericRepository<Rental> _repositoryRental;
        private readonly IGenericRepository<Car> _repositoryCar;

        public Rentals1Controller(CarRentalDBContext context, IGenericRepository<Rental> repositoryRental, IGenericRepository<Car> repositoryCar)
        {
            _context = context;
            _repositoryRental = repositoryRental;
            _repositoryCar = repositoryCar;
        }

        // GET: Rentals1
        public async Task<IActionResult> Index()
        {
            var carRentalDBContext = _context.Rentals.Include(r => r.Car).Include(r => r.Driver);
            return View(await carRentalDBContext.ToListAsync());
        }

        // GET: Rentals1/Details/5
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

        // GET: Rentals1/Create
        public IActionResult Create()
        {
            var rentals = _repositoryRental.GetAll();
            var cars = _repositoryCar.GetAll();

            var list = new List<int>();
            foreach(var rental in rentals)
            {
                if (list.Contains(rental.CarId))
                    continue;
                if(rental.ReturnDate == null)
                {
                    list.Add(rental.CarId);
                }
            }
            IList<int> idList = cars.Select(p => p.Id).ToList();
            foreach(var rental in rentals)
            {
                if(list.Contains(rental.CarId))
                {
                    idList.Remove(rental.CarId);
                }
            }

            ViewData["CarId"] = new SelectList(idList);
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Id");

            return View();
        }

        // POST: Rentals1/Create
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
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Make", rental.CarId);
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Email", rental.DriverId);
            return View(rental);
        }

        // GET: Rentals1/Edit/5
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
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", rental.CarId);
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Id", rental.DriverId);
            return View(rental);
        }

        // POST: Rentals1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarId,DriverId,RentDate,ReturnDate,Comments")] Rental rental)
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
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", rental.CarId);
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Id", rental.DriverId);
            return View(rental);
        }

        // GET: Rentals1/Delete/5
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

        // POST: Rentals1/Delete/5
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
    }
}
