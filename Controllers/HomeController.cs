using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KarnelTravels.Data;
using KarnelTravels.Models;

namespace KarnelTravels.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var featuredTours = await _context.Tours
            .Where(t => t.IsFeatured && t.IsActive)
            .Take(6)
            .ToListAsync();

        var popularSpots = await _context.TouristSpots
            .Where(s => s.IsActive)
            .Take(6)
            .ToListAsync();

        var hotels = await _context.Hotels
            .Where(h => h.IsActive)
            .Take(6)
            .ToListAsync();

        var promotions = await _context.Promotions
            .Where(p => p.IsActive && p.ShowOnHome && p.EndDate > DateTime.Now)
            .Take(4)
            .ToListAsync();

        ViewData["FeaturedTours"] = featuredTours;
        ViewData["PopularSpots"] = popularSpots;
        ViewData["Hotels"] = hotels;
        ViewData["Promotions"] = promotions;

        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Contact(Contact contact)
    {
        if (ModelState.IsValid)
        {
            contact.CreatedAt = DateTime.Now;
            contact.IsRead = false;
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi sớm nhất có thể.";
            return RedirectToAction(nameof(Contact));
        }
        return View(contact);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
