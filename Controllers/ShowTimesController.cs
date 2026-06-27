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
    public class ShowTimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShowTimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShowTimes
        public async Task<IActionResult> Index(string searchString)
        {
            var showTimes = _context.ShowTimes
                .Include(s => s.Movie)
                .Include(s => s.Hall);

            if (!String.IsNullOrEmpty(searchString))
            {
                var filtered = showTimes.Where(s => s.Movie.Title.Contains(searchString)
                                                  || s.Hall.HallName.Contains(searchString));
                ViewData["CurrentFilter"] = searchString;
                return View(await filtered.ToListAsync());
            }

            return View(await showTimes.ToListAsync());
        }

        // GET: ShowTimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showTime = await _context.ShowTimes
                .Include(s => s.Hall)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.ShowTimeID == id);
            if (showTime == null)
            {
                return NotFound();
            }

            return View(showTime);
        }

        // GET: ShowTimes/Create
        public IActionResult Create()
        {
            ViewData["HallID"] = new SelectList(_context.CinemaHalls, "HallID", "HallID");
            ViewData["MovieID"] = new SelectList(_context.Movies, "MovieID", "MovieID");
            return View();
        }

        // POST: ShowTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShowTimeID,MovieID,HallID,Date,Time,Price,Status")] ShowTime showTime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(showTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HallID"] = new SelectList(_context.CinemaHalls, "HallID", "HallID", showTime.HallID);
            ViewData["MovieID"] = new SelectList(_context.Movies, "MovieID", "MovieID", showTime.MovieID);
            return View(showTime);
        }

        // GET: ShowTimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showTime = await _context.ShowTimes.FindAsync(id);
            if (showTime == null)
            {
                return NotFound();
            }
            ViewData["HallID"] = new SelectList(_context.CinemaHalls, "HallID", "HallID", showTime.HallID);
            ViewData["MovieID"] = new SelectList(_context.Movies, "MovieID", "MovieID", showTime.MovieID);
            return View(showTime);
        }

        // POST: ShowTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShowTimeID,MovieID,HallID,Date,Time,Price,Status")] ShowTime showTime)
        {
            if (id != showTime.ShowTimeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(showTime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowTimeExists(showTime.ShowTimeID))
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
            ViewData["HallID"] = new SelectList(_context.CinemaHalls, "HallID", "HallID", showTime.HallID);
            ViewData["MovieID"] = new SelectList(_context.Movies, "MovieID", "MovieID", showTime.MovieID);
            return View(showTime);
        }

        // GET: ShowTimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showTime = await _context.ShowTimes
                .Include(s => s.Hall)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.ShowTimeID == id);
            if (showTime == null)
            {
                return NotFound();
            }

            return View(showTime);
        }

        // POST: ShowTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var showTime = await _context.ShowTimes.FindAsync(id);
            if (showTime != null)
            {
                _context.ShowTimes.Remove(showTime);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShowTimeExists(int id)
        {
            return _context.ShowTimes.Any(e => e.ShowTimeID == id);
        }
    }
}
