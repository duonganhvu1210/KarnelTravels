namespace KarnelTravels.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KarnelTravels.Data;
using KarnelTravels.Models;

public class TouristSpotController : Controller
{
    private readonly KarnelDbContext _context;

    public TouristSpotController(KarnelDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string region, string type)
    {
        var spots = await _context.TouristSpots.Where(s => s.IsActive).ToListAsync();

        if (!string.IsNullOrEmpty(region))
            spots = spots.Where(s => s.Region == region).ToList();

        if (!string.IsNullOrEmpty(type))
            spots = spots.Where(s => s.SpotType == type).ToList();

        return View(spots);
    }

    public async Task<IActionResult> Details(int id)
    {
        var spot = await _context.TouristSpots.FindAsync(id);
        if (spot == null) return NotFound();
        return View(spot);
    }
}

public class TourController : Controller
{
    private readonly KarnelDbContext _context;

    public TourController(KarnelDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string destination, int? minPrice, int? maxPrice)
    {
        var tours = await _context.Tours.Where(t => t.IsActive).ToListAsync();

        if (!string.IsNullOrEmpty(destination))
            tours = tours.Where(t => t.Destination != null && t.Destination.Contains(destination)).ToList();

        if (minPrice.HasValue)
            tours = tours.Where(t => t.Price >= minPrice.Value).ToList();

        if (maxPrice.HasValue)
            tours = tours.Where(t => t.Price <= maxPrice.Value).ToList();

        return View(tours);
    }

    public async Task<IActionResult> Details(int id)
    {
        var tour = await _context.Tours.FindAsync(id);
        if (tour == null) return NotFound();
        return View(tour);
    }
}

public class HotelController : Controller
{
    private readonly KarnelDbContext _context;

    public HotelController(KarnelDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string city, int? starRating)
    {
        var hotels = await _context.Hotels.Where(h => h.IsActive).ToListAsync();

        if (!string.IsNullOrEmpty(city))
            hotels = hotels.Where(h => h.City == city).ToList();

        if (starRating.HasValue)
            hotels = hotels.Where(h => h.StarRating == starRating.Value).ToList();

        return View(hotels);
    }

    public async Task<IActionResult> Details(int id)
    {
        var hotel = await _context.Hotels.Include(h => h.Rooms).FirstOrDefaultAsync(h => h.HotelId == id);
        if (hotel == null) return NotFound();
        return View(hotel);
    }
}

public class RestaurantController : Controller
{
    private readonly KarnelDbContext _context;

    public RestaurantController(KarnelDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string city, string cuisineType)
    {
        var restaurants = await _context.Restaurants.Where(r => r.IsActive).ToListAsync();

        if (!string.IsNullOrEmpty(city))
            restaurants = restaurants.Where(r => r.City == city).ToList();

        if (!string.IsNullOrEmpty(cuisineType))
            restaurants = restaurants.Where(r => r.CuisineType == cuisineType).ToList();

        return View(restaurants);
    }

    public async Task<IActionResult> Details(int id)
    {
        var restaurant = await _context.Restaurants.FindAsync(id);
        if (restaurant == null) return NotFound();
        return View(restaurant);
    }
}

public class ResortController : Controller
{
    private readonly KarnelDbContext _context;

    public ResortController(KarnelDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string city, string resortType)
    {
        var resorts = await _context.Resorts.Where(r => r.IsActive).ToListAsync();

        if (!string.IsNullOrEmpty(city))
            resorts = resorts.Where(r => r.City == city).ToList();

        if (!string.IsNullOrEmpty(resortType))
            resorts = resorts.Where(r => r.ResortType == resortType).ToList();

        return View(resorts);
    }

    public async Task<IActionResult> Details(int id)
    {
        var resort = await _context.Resorts.Include(r => r.Rooms).FirstOrDefaultAsync(r => r.ResortId == id);
        if (resort == null) return NotFound();
        return View(resort);
    }
}

public class TransportController : Controller
{
    private readonly KarnelDbContext _context;

    public TransportController(KarnelDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string fromLocation, string toLocation, string transportType)
    {
        var transports = await _context.Transports.Where(t => t.IsActive).ToListAsync();

        if (!string.IsNullOrEmpty(fromLocation))
            transports = transports.Where(t => t.FromLocation == fromLocation).ToList();

        if (!string.IsNullOrEmpty(toLocation))
            transports = transports.Where(t => t.ToLocation == toLocation).ToList();

        if (!string.IsNullOrEmpty(transportType))
            transports = transports.Where(t => t.TransportType == transportType).ToList();

        return View(transports);
    }

    public async Task<IActionResult> Details(int id)
    {
        var transport = await _context.Transports.FindAsync(id);
        if (transport == null) return NotFound();
        return View(transport);
    }
}
