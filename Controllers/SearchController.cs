namespace KarnelTravels.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KarnelTravels.Data;
using KarnelTravels.Models;

public class SearchController : Controller
{
    private readonly KarnelDbContext _context;

    public SearchController(KarnelDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string query, string type)
    {
        ViewData["Query"] = query;
        ViewData["Type"] = type;

        if (string.IsNullOrEmpty(query) && string.IsNullOrEmpty(type))
        {
            return View();
        }

        var spots = await _context.TouristSpots.Where(s => s.IsActive).ToListAsync();
        var hotels = await _context.Hotels.Where(h => h.IsActive).ToListAsync();
        var tours = await _context.Tours.Where(t => t.IsActive).ToListAsync();
        var restaurants = await _context.Restaurants.Where(r => r.IsActive).ToListAsync();
        var resorts = await _context.Resorts.Where(r => r.IsActive).ToListAsync();
        var transports = await _context.Transports.Where(t => t.IsActive).ToListAsync();

        if (!string.IsNullOrEmpty(query))
        {
            query = query.ToLower();
            spots = spots.Where(s => s.Name.ToLower().Contains(query) || (s.Description?.ToLower().Contains(query) ?? false)).ToList();
            hotels = hotels.Where(h => h.Name.ToLower().Contains(query) || (h.Description?.ToLower().Contains(query) ?? false)).ToList();
            tours = tours.Where(t => t.Name.ToLower().Contains(query) || (t.Description?.ToLower().Contains(query) ?? false)).ToList();
            restaurants = restaurants.Where(r => r.Name.ToLower().Contains(query) || (r.Description?.ToLower().Contains(query) ?? false)).ToList();
            resorts = resorts.Where(r => r.Name.ToLower().Contains(query) || (r.Description?.ToLower().Contains(query) ?? false)).ToList();
            transports = transports.Where(t => t.Name.ToLower().Contains(query) || (t.Route?.ToLower().Contains(query) ?? false)).ToList();
        }

        ViewData["Spots"] = spots;
        ViewData["Hotels"] = hotels;
        ViewData["Tours"] = tours;
        ViewData["Restaurants"] = restaurants;
        ViewData["Resorts"] = resorts;
        ViewData["Transports"] = transports;

        return View();
    }
}

public class BookingController : Controller
{
    private readonly KarnelDbContext _context;

    public BookingController(KarnelDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Booking booking)
    {
        if (ModelState.IsValid)
        {
            booking.BookingDate = DateTime.Now;
            booking.Status = "Pending";
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đặt dịch vụ thành công! Chúng tôi sẽ liên hệ với bạn sớm.";
            return RedirectToAction(nameof(Index));
        }
        return View(booking);
    }

    public async Task<IActionResult> MyBookings()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Account");
        }
        
        var bookings = await _context.Bookings
            .Include(b => b.Tour)
            .Include(b => b.Hotel)
            .Where(b => b.UserId == userId.Value)
            .ToListAsync();
            
        return View(bookings);
    }
}
