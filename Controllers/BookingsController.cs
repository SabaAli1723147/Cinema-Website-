using CinemaBookingApp.Data;
using CinemaBookingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingApp.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Bookings (Admin use)
        public async Task<IActionResult> Index(string searchString)
        {
            var bookings = _context.Bookings
                .Include(b => b.ShowTime)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.ShowTime)
                    .ThenInclude(s => s.Hall)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b =>
                    b.Status.Contains(searchString));
            }

            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.ShowTime)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["ShowTimeID"] = new SelectList(_context.ShowTimes, "ShowTimeID", "ShowTimeID");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingID,ShowTimeID,BookingDate,TotalPrice,Status")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction("Payment", "Bookings");
            }
            ViewData["ShowTimeID"] = new SelectList(_context.ShowTimes, "ShowTimeID", "ShowTimeID", booking.ShowTimeID);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewData["ShowTimeID"] = new SelectList(_context.ShowTimes, "ShowTimeID", "ShowTimeID", booking.ShowTimeID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingID,ShowTimeID,BookingDate,TotalPrice,Status")] Booking booking)
        {
            if (id != booking.BookingID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShowTimeID"] = new SelectList(_context.ShowTimes, "ShowTimeID", "ShowTimeID", booking.ShowTimeID);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.ShowTime)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null) _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ── CUSTOMER FLOW ──

        // Seat Selection (no login required — guest can view)
        public async Task<IActionResult> SelectSeats(int showTimeId)
        {
            var showTime = await _context.ShowTimes
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .FirstOrDefaultAsync(s => s.ShowTimeID == showTimeId);

            if (showTime == null) return NotFound();

            var seats = await _context.Seats
                .Where(s => s.HallID == showTime.HallID)
                .ToListAsync();

            ViewBag.ShowTime = showTime;
            ViewBag.TicketPrice = showTime.Price;
            return View(seats);
        }

        // GET: Bookings/ConfirmBooking
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ConfirmBooking()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var latestBooking = await _context.Bookings
                .Include(b => b.ShowTime)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.ShowTime)
                    .ThenInclude(s => s.Hall)
                .Where(b => b.UserID == user.Id)
                .OrderByDescending(b => b.BookingDate)
                .FirstOrDefaultAsync();

            if (latestBooking == null) return RedirectToAction("Index", "Home");

            // تأكدي أن الاسم هنا يطابق تماماً اسم ملف الـ View الخاص بك
            return View("BookingSummary", latestBooking);
        }

        // POST: Bookings/ConfirmBooking
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBooking(int showTimeId, int seatId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var showTime = await _context.ShowTimes
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .FirstOrDefaultAsync(s => s.ShowTimeID == showTimeId);

            if (showTime == null) return NotFound();

            var booking = new Booking
            {
                ShowTimeID = showTimeId,
                UserID = user.Id,
                BookingDate = DateTime.Now,
                TotalPrice = showTime.Price,
                Status = "Confirmed"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            booking.ShowTime = showTime;

            // تأكدي أن الاسم هنا يطابق تماماً اسم ملف الـ View الخاص بك
            return View("BookingSummary", booking);
        }

        // My Bookings (customer sees only their own bookings)
        [Authorize]
        public async Task<IActionResult> MyBookings()
        {
            var user = await _userManager.GetUserAsync(User);
            var bookings = await _context.Bookings
                .Include(b => b.ShowTime)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.ShowTime)
                    .ThenInclude(s => s.Hall)
                .Where(b => b.UserID == user.Id)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }
        // Payment Page
        public IActionResult Payment()
        {
            return View();
        }
        // Ticket Page
        public IActionResult Ticket()
        {
            return View();
        }
        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingID == id);
        }
    }
}