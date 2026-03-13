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
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfLastMonth = startOfMonth.AddMonths(-1);
            var endOfLastMonth = startOfMonth.AddDays(-1);

            // F161-F164: Thống kê tổng quan
            var viewModel = new DashboardViewModel
            {
                TotalDestinations = await _context.TouristSpots.CountAsync(),
                TotalHotels = await _context.Hotels.CountAsync(),
                TotalRestaurants = await _context.Restaurants.CountAsync(),
                TotalResorts = await _context.Resorts.CountAsync(),
                TotalTours = await _context.Tours.CountAsync(),
                TotalTransports = await _context.Transports.CountAsync(),

                // F165: Đơn hàng hôm nay
                TodayOrdersCount = await _context.Bookings
                    .CountAsync(b => b.BookingDate.Date == today),

                // F166: Doanh thu tháng này
                MonthlyRevenue = await _context.Bookings
                    .Where(b => b.BookingDate >= startOfMonth && b.IsPaid)
                    .SumAsync(b => b.TotalAmount),

                // F167: Danh sách đơn hàng gần đây (10 đơn)
                RecentBookings = await _context.Bookings
                    .Include(b => b.Tour)
                    .Include(b => b.Hotel)
                    .OrderByDescending(b => b.BookingDate)
                    .Take(10)
                    .ToListAsync(),

                // F170: Tổng số đơn hàng
                TotalBookings = await _context.Bookings.CountAsync(),

                // Tổng doanh thu
                TotalRevenue = await _context.Bookings
                    .Where(b => b.IsPaid)
                    .SumAsync(b => b.TotalAmount),

                // F169: Liên hệ chưa đọc
                UnreadContacts = await _context.Contacts
                    .Where(c => !c.IsRead)
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(5)
                    .ToListAsync()
            };

            // F168: Dữ liệu biểu đồ doanh thu 12 tháng
            viewModel.MonthlyRevenues = await GetMonthlyRevenueDataAsync();

            return View(viewModel);
        }

        private async Task<List<MonthlyRevenueData>> GetMonthlyRevenueDataAsync()
        {
            var result = new List<MonthlyRevenueData>();
            var today = DateTime.Today;

            for (int i = 11; i >= 0; i--)
            {
                var targetDate = today.AddMonths(-i);
                var startOfMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                var revenue = await _context.Bookings
                    .Where(b => b.BookingDate >= startOfMonth && b.BookingDate <= endOfMonth && b.IsPaid)
                    .SumAsync(b => b.TotalAmount);

                result.Add(new MonthlyRevenueData
                {
                    Month = targetDate.ToString("MMM yyyy"),
                    Revenue = revenue
                });
            }

            return result;
        }

        // F167: Quản lý đơn đặt
        public async Task<IActionResult> ManageBookings(string? status = null)
        {
            var bookings = _context.Bookings
                .Include(b => b.Tour)
                .Include(b => b.Hotel)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                bookings = bookings.Where(b => b.Status == status);
            }

            var model = await bookings
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            ViewData["Status"] = status;
            return View(model);
        }

        // Cập nhật trạng thái đơn hàng
        public async Task<IActionResult> UpdateBookingStatus(int id, string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = status;
                await _context.SaveChangesAsync();

                // TODO: F170 - Gửi thông báo real-time qua SignalR khi có cập nhật
                // await _hubContext.Clients.User(booking.UserId).SendAsync("BookingStatusChanged", status);
            }
            return RedirectToAction(nameof(ManageBookings));
        }

        // F169: Quản lý liên hệ
        public async Task<IActionResult> ManageContacts()
        {
            var contacts = await _context.Contacts
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
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

        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageContacts));
        }

        // Quản lý điểm du lịch
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

        public async Task<IActionResult> EditSpot(int id)
        {
            var spot = await _context.TouristSpots.FindAsync(id);
            return View(spot);
        }

        [HttpPost]
        public async Task<IActionResult> EditSpot(TouristSpot spot)
        {
            _context.TouristSpots.Update(spot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageSpots));
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

        // Quản lý khách sạn
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

        public async Task<IActionResult> EditHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            return View(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> EditHotel(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageHotels));
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

        // Quản lý Tours
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

        public async Task<IActionResult> EditTour(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            return View(tour);
        }

        [HttpPost]
        public async Task<IActionResult> EditTour(Tour tour)
        {
            _context.Tours.Update(tour);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageTours));
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

        // Quản lý khuyến mãi
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

        public async Task<IActionResult> EditPromotion(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            return View(promotion);
        }

        [HttpPost]
        public async Task<IActionResult> EditPromotion(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManagePromotions));
        }

        public async Task<IActionResult> DeletePromotion(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManagePromotions));
        }
    }
}
