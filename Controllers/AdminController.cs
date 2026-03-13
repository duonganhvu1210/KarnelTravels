namespace KarnelTravels.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KarnelTravels.Data;
using KarnelTravels.Models;

public class AdminController : Controller
{
    private readonly KarnelDbContext _context;

    public AdminController(KarnelDbContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Role == "Admin");
        
        if (user != null && user.PasswordHash == password)
        {
            HttpContext.Session.SetInt32("AdminId", user.UserId);
            HttpContext.Session.SetString("AdminName", user.FullName);
            return RedirectToAction("Dashboard");
        }
        
        ViewData["Error"] = "Email hoặc mật khẩu không đúng!";
        return View();
    }

    public async Task<IActionResult> Dashboard()
    {
        var adminId = HttpContext.Session.GetInt32("AdminId");
        if (!adminId.HasValue)
            return RedirectToAction("Login");

        var stats = new
        {
            TotalSpots = await _context.TouristSpots.CountAsync(),
            TotalHotels = await _context.Hotels.CountAsync(),
            TotalTours = await _context.Tours.CountAsync(),
            TotalBookings = await _context.Bookings.CountAsync(),
            TotalRevenue = await _context.Bookings.Where(b => b.IsPaid).SumAsync(b => b.TotalAmount),
            UnreadContacts = await _context.Contacts.CountAsync(c => !c.IsRead),
            RecentBookings = await _context.Bookings
                .Include(b => b.Tour)
                .OrderByDescending(b => b.BookingDate)
                .Take(5)
                .ToListAsync()
        };

        ViewData["Stats"] = stats;
        return View();
    }

    public async Task<IActionResult> ManageSpots()
    {
        var adminId = HttpContext.Session.GetInt32("AdminId");
        if (!adminId.HasValue)
            return RedirectToAction("Login");

        var spots = await _context.TouristSpots.ToListAsync();
        return View(spots);
    }

    [HttpPost]
    public async Task<IActionResult> AddSpot(TouristSpot spot)
    {
        var adminId = HttpContext.Session.GetInt32("AdminId");
        if (!adminId.HasValue)
            return RedirectToAction("Login");

        spot.CreatedAt = DateTime.Now;
        _context.TouristSpots.Add(spot);
        await _context.SaveChangesAsync();
        return RedirectToAction("ManageSpots");
    }

    public async Task<IActionResult> ManageHotels()
    {
        var adminId = HttpContext.Session.GetInt32("AdminId");
        if (!adminId.HasValue)
            return RedirectToAction("Login");

        var hotels = await _context.Hotels.Include(h => h.Rooms).ToListAsync();
        return View(hotels);
    }

    [HttpPost]
    public async Task<IActionResult> AddHotel(Hotel hotel)
    {
        var adminId = HttpContext.Session.GetInt32("AdminId");
        if (!adminId.HasValue)
            return RedirectToAction("Login");

        hotel.CreatedAt = DateTime.Now;
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();
        return RedirectToAction("ManageHotels");
    }

    public async Task<IActionResult> ManageTours()
    {
        var adminId = HttpContext.Session.GetInt32("AdminId");
        if (!adminId.HasValue)
            return RedirectToAction("Login");

        var tours = await _context.Tours.ToListAsync();
        return View(tours);
    }

    [HttpPost]
    public async Task<IActionResult> AddTour(Tour tour)
    {
        var adminId = HttpContext.Session.GetInt32("AdminId");
        if (!adminId.HasValue)
            return RedirectToAction("Login");

        tour.CreatedAt = DateTime.Now;
        _context.Tours.Add(tour);
        await _context.SaveChangesAsync();
        return RedirectToAction("ManageTours");
    }

    public async Task<IActionResult> ManageBookings()
    {
        var adminId = HttpContext.Session.GetInt32("AdminId");
        if (!adminId.HasValue)
            return RedirectToAction("Login");

        var bookings = await _context.Bookings
            .Include(b => b.Tour)
            .Include(b => b.Hotel)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
        return View(bookings);
    }

    public async Task<IActionResult> UpdateBookingStatus(int id, string status)
    {
        var adminId = HttpContext.Session.GetInt32("AdminId");
        if (!adminId.HasValue)
            return RedirectToAction("Login");

        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            booking.Status = status;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("ManageBookings");
    }

    public async Task<IActionResult> ManageContacts()
    {
        var adminId = HttpContext.Session.GetInt32("AdminId");
        if (!adminId.HasValue)
            return RedirectToAction("Login");

        var contacts = await _context.Contacts.OrderByDescending(c => c.CreatedAt).ToListAsync();
        return View(contacts);
    }

    public async Task<IActionResult> MarkContactRead(int id)
    {
        var adminId = HttpContext.Session.GetInt32("AdminId");
        if (!adminId.HasValue)
            return RedirectToAction("Login");

        var contact = await _context.Contacts.FindAsync(id);
        if (contact != null)
        {
            contact.IsRead = true;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("ManageContacts");
    }

    public async Task<IActionResult> ManagePromotions()
    {
        var adminId = HttpContext.Session.GetInt32("AdminId");
        if (!adminId.HasValue)
            return RedirectToAction("Login");

        var promotions = await _context.Promotions.ToListAsync();
        return View(promotions);
    }

    [HttpPost]
    public async Task<IActionResult> AddPromotion(Promotion promotion)
    {
        var adminId = HttpContext.Session.GetInt32("AdminId");
        if (!adminId.HasValue)
            return RedirectToAction("Login");

        promotion.CreatedAt = DateTime.Now;
        _context.Promotions.Add(promotion);
        await _context.SaveChangesAsync();
        return RedirectToAction("ManagePromotions");
    }

    public async Task<IActionResult> DeleteSpot(int id)
    {
        var spot = await _context.TouristSpots.FindAsync(id);
        if (spot != null)
        {
            _context.TouristSpots.Remove(spot);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("ManageSpots");
    }

    public async Task<IActionResult> DeleteHotel(int id)
    {
        var hotel = await _context.Hotels.FindAsync(id);
        if (hotel != null)
        {
            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("ManageHotels");
    }

    public async Task<IActionResult> DeleteTour(int id)
    {
        var tour = await _context.Tours.FindAsync(id);
        if (tour != null)
        {
            _context.Tours.Remove(tour);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("ManageTours");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
