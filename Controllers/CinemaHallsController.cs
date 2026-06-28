using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaBookingApp.Data;
using CinemaBookingApp.Models;

namespace CinemaBookingApp.Controllers
{
    public class CinemaHallsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CinemaHallsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CinemaHalls
        public async Task<IActionResult> Index(string searchString)
        {
            var halls = from h in _context.CinemaHalls
                        select h;

            if (!string.IsNullOrEmpty(searchString))
            {
                halls = halls.Where(h => h.HallName.Contains(searchString));
            }

            return View(await halls.ToListAsync());
        }

        // GET: CinemaHalls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinemaHall = await _context.CinemaHalls
                .FirstOrDefaultAsync(m => m.HallID == id);
            if (cinemaHall == null)
            {
                return NotFound();
            }

            return View(cinemaHall);
        }

        // GET: CinemaHalls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CinemaHalls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HallID,HallName,Capacity")] CinemaHall cinemaHall)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cinemaHall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cinemaHall);
        }

        // GET: CinemaHalls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinemaHall = await _context.CinemaHalls.FindAsync(id);
            if (cinemaHall == null)
            {
                return NotFound();
            }
            return View(cinemaHall);
        }

        // POST: CinemaHalls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HallID,HallName,Capacity")] CinemaHall cinemaHall)
        {
            if (id != cinemaHall.HallID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cinemaHall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CinemaHallExists(cinemaHall.HallID))
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
            return View(cinemaHall);
        }

        // GET: CinemaHalls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinemaHall = await _context.CinemaHalls
                .FirstOrDefaultAsync(m => m.HallID == id);
            if (cinemaHall == null)
            {
                return NotFound();
            }

            return View(cinemaHall);
        }

        // POST: CinemaHalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinemaHall = await _context.CinemaHalls.FindAsync(id);
            if (cinemaHall != null)
            {
                _context.CinemaHalls.Remove(cinemaHall);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CinemaHallExists(int id)
        {
            return _context.CinemaHalls.Any(e => e.HallID == id);
        }
    }
}
