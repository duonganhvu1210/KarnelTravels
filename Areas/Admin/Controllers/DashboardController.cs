using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KarnelTravels.Data;
using KarnelTravels.Models;
using KarnelTravels.Models.ViewModels;

namespace KarnelTravels.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
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

            return View(viewModel);
        }

        public async Task<IActionResult> ManageSpots()
        {
            var spots = await _context.TouristSpots.ToListAsync();
            return View(spots);
        }

        [HttpPost]
        public async Task<IActionResult> AddSpot(TouristSpot spot)
        {
            spot.CreatedAt = DateTime.Now;
            _context.TouristSpots.Add(spot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageSpots));
        }

        public async Task<IActionResult> ManageHotels()
        {
            var hotels = await _context.Hotels.Include(h => h.Rooms).ToListAsync();
            return View(hotels);
        }

        [HttpPost]
        public async Task<IActionResult> AddHotel(Hotel hotel)
        {
            hotel.CreatedAt = DateTime.Now;
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageHotels));
        }

        public async Task<IActionResult> ManageTours()
        {
            var tours = await _context.Tours.ToListAsync();
            return View(tours);
        }

        [HttpPost]
        public async Task<IActionResult> AddTour(Tour tour)
        {
            tour.CreatedAt = DateTime.Now;
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageTours));
        }

        public async Task<IActionResult> ManageBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.Hotel)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
            return View(bookings);
        }

        public async Task<IActionResult> UpdateBookingStatus(int id, string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = status;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageBookings));
        }

        public async Task<IActionResult> ManageContacts()
        {
            var contacts = await _context.Contacts.OrderByDescending(c => c.CreatedAt).ToListAsync();
            return View(contacts);
        }

        public async Task<IActionResult> MarkContactRead(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                contact.IsRead = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageContacts));
        }

        public async Task<IActionResult> ManagePromotions()
        {
            var promotions = await _context.Promotions.ToListAsync();
            return View(promotions);
        }

        [HttpPost]
        public async Task<IActionResult> AddPromotion(Promotion promotion)
        {
            promotion.CreatedAt = DateTime.Now;
            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManagePromotions));
        }

        public async Task<IActionResult> DeleteSpot(int id)
        {
            var spot = await _context.TouristSpots.FindAsync(id);
            if (spot != null)
            {
                _context.TouristSpots.Remove(spot);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageSpots));
        }

        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageHotels));
        }

        public async Task<IActionResult> DeleteTour(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageTours));
        }
    }
}
